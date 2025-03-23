import {
  DateOnly,
  Duration,
  TimeOnly,
  createBackedModelProxyHandler,
  createUntypedArray,
  createUntypedBoolean,
  createUntypedNodeFromDiscriminatorValue,
  createUntypedNull,
  createUntypedNumber,
  createUntypedObject,
  createUntypedString,
  getEnumValueFromStringValue,
  inNodeEnv,
  isBackingStoreEnabled,
  isUntypedArray,
  isUntypedBoolean,
  isUntypedNode,
  isUntypedNull,
  isUntypedNumber,
  isUntypedObject,
  isUntypedString,
  parseGuidString
} from "./chunk-K64FEM4K.js";
import "./chunk-TXDUYLVM.js";

// node_modules/@microsoft/kiota-serialization-json/dist/es/src/jsonParseNode.js
var JsonParseNode = class _JsonParseNode {
  constructor(_jsonNode) {
    this._jsonNode = _jsonNode;
    this.getStringValue = () => typeof this._jsonNode === "string" ? this._jsonNode : void 0;
    this.getChildNode = (identifier) => this._jsonNode && typeof this._jsonNode === "object" && this._jsonNode[identifier] !== void 0 ? new _JsonParseNode(this._jsonNode[identifier]) : void 0;
    this.getBooleanValue = () => typeof this._jsonNode === "boolean" ? this._jsonNode : void 0;
    this.getNumberValue = () => typeof this._jsonNode === "number" ? this._jsonNode : void 0;
    this.getGuidValue = () => parseGuidString(this.getStringValue());
    this.getDateValue = () => this._jsonNode ? new Date(this._jsonNode) : void 0;
    this.getDateOnlyValue = () => DateOnly.parse(this.getStringValue());
    this.getTimeOnlyValue = () => TimeOnly.parse(this.getStringValue());
    this.getDurationValue = () => Duration.parse(this.getStringValue());
    this.getCollectionOfPrimitiveValues = () => {
      if (!Array.isArray(this._jsonNode)) {
        return void 0;
      }
      return this._jsonNode.map((x) => {
        const currentParseNode = new _JsonParseNode(x);
        const typeOfX = typeof x;
        if (typeOfX === "boolean") {
          return currentParseNode.getBooleanValue();
        } else if (typeOfX === "string") {
          return currentParseNode.getStringValue();
        } else if (typeOfX === "number") {
          return currentParseNode.getNumberValue();
        } else if (x instanceof Date) {
          return currentParseNode.getDateValue();
        } else if (x instanceof DateOnly) {
          return currentParseNode.getDateValue();
        } else if (x instanceof TimeOnly) {
          return currentParseNode.getDateValue();
        } else if (x instanceof Duration) {
          return currentParseNode.getDateValue();
        } else {
          throw new Error(`encountered an unknown type during deserialization ${typeof x}`);
        }
      });
    };
    this.getCollectionOfObjectValues = (method) => {
      if (!Array.isArray(this._jsonNode)) {
        return void 0;
      }
      return this._jsonNode ? this._jsonNode.map((x) => new _JsonParseNode(x)).map((x) => x.getObjectValue(method)) : void 0;
    };
    this.getObjectValue = (parsableFactory) => {
      const temp = {};
      if (isUntypedNode(parsableFactory(this)(temp))) {
        const valueType = typeof this._jsonNode;
        let value = temp;
        if (valueType === "boolean") {
          value = createUntypedBoolean(this._jsonNode);
        } else if (valueType === "string") {
          value = createUntypedString(this._jsonNode);
        } else if (valueType === "number") {
          value = createUntypedNumber(this._jsonNode);
        } else if (Array.isArray(this._jsonNode)) {
          const nodes = [];
          this._jsonNode.forEach((x) => {
            nodes.push(new _JsonParseNode(x).getObjectValue(createUntypedNodeFromDiscriminatorValue));
          });
          value = createUntypedArray(nodes);
        } else if (this._jsonNode && valueType === "object") {
          const properties = {};
          Object.entries(this._jsonNode).forEach(([k, v]) => {
            properties[k] = new _JsonParseNode(v).getObjectValue(createUntypedNodeFromDiscriminatorValue);
          });
          value = createUntypedObject(properties);
        } else if (!this._jsonNode) {
          value = createUntypedNull();
        }
        return value;
      }
      const enableBackingStore = isBackingStoreEnabled(parsableFactory(this)(temp));
      const objectValue = enableBackingStore ? new Proxy(temp, createBackedModelProxyHandler()) : temp;
      if (this.onBeforeAssignFieldValues) {
        this.onBeforeAssignFieldValues(objectValue);
      }
      this.assignFieldValues(objectValue, parsableFactory);
      if (this.onAfterAssignFieldValues) {
        this.onAfterAssignFieldValues(objectValue);
      }
      return objectValue;
    };
    this.assignFieldValues = (model, parsableFactory) => {
      const fields = parsableFactory(this)(model);
      if (!this._jsonNode) return;
      Object.entries(this._jsonNode).forEach(([k, v]) => {
        const deserializer = fields[k];
        if (deserializer) {
          deserializer(new _JsonParseNode(v));
        } else {
          model[k] = v;
        }
      });
    };
    this.getCollectionOfEnumValues = (type) => {
      if (Array.isArray(this._jsonNode)) {
        return this._jsonNode.map((x) => {
          const node = new _JsonParseNode(x);
          return node.getEnumValue(type);
        }).filter(Boolean);
      }
      return [];
    };
    this.getEnumValue = (type) => {
      const rawValue = this.getStringValue();
      if (!rawValue) {
        return void 0;
      }
      return getEnumValueFromStringValue(rawValue, type);
    };
  }
  getByteArrayValue() {
    const strValue = this.getStringValue();
    if (strValue && strValue.length > 0) {
      return inNodeEnv() ? Buffer.from(strValue, "base64").buffer : new TextEncoder().encode(strValue);
    }
    return void 0;
  }
};

// node_modules/@microsoft/kiota-serialization-json/dist/es/src/jsonSerializationWriter.js
var JsonSerializationWriter = class _JsonSerializationWriter {
  constructor() {
    this.writer = [];
    this.shouldWriteValueOrNull = (key, value) => {
      if (value === null) {
        this.writeNullValue(key);
        return false;
      }
      return true;
    };
    this.writeStringValue = (key, value) => {
      if (value === void 0) {
        return;
      }
      if (this.shouldWriteValueOrNull(key, value)) {
        key && this.writePropertyName(key);
        this.writer.push(JSON.stringify(value));
        key && this.writer.push(_JsonSerializationWriter.propertySeparator);
      }
    };
    this.writePropertyName = (key) => {
      this.writer.push(`"${key}":`);
    };
    this.writeBooleanValue = (key, value) => {
      if (value === void 0) {
        return;
      }
      if (this.shouldWriteValueOrNull(key, value)) {
        key && this.writePropertyName(key);
        this.writer.push(`${value}`);
        key && this.writer.push(_JsonSerializationWriter.propertySeparator);
      }
    };
    this.writeNumberValue = (key, value) => {
      if (value === void 0) {
        return;
      }
      if (this.shouldWriteValueOrNull(key, value)) {
        key && this.writePropertyName(key);
        this.writer.push(`${value}`);
        key && this.writer.push(_JsonSerializationWriter.propertySeparator);
      }
    };
    this.writeGuidValue = (key, value) => {
      if (value === void 0) {
        return;
      }
      if (this.shouldWriteValueOrNull(key, value)) {
        key && this.writePropertyName(key);
        this.writer.push(`"${value}"`);
        key && this.writer.push(_JsonSerializationWriter.propertySeparator);
      }
    };
    this.writeDateValue = (key, value) => this.writeStringValue(key, value === null ? null : value === null || value === void 0 ? void 0 : value.toISOString());
    this.writeDateOnlyValue = (key, value) => this.writeStringValue(key, value === null ? null : value === null || value === void 0 ? void 0 : value.toString());
    this.writeTimeOnlyValue = (key, value) => this.writeStringValue(key, value === null ? null : value === null || value === void 0 ? void 0 : value.toString());
    this.writeDurationValue = (key, value) => this.writeStringValue(key, value === null ? null : value === null || value === void 0 ? void 0 : value.toString());
    this.writeNullValue = (key) => {
      key && this.writePropertyName(key);
      this.writer.push(`null`);
      key && this.writer.push(_JsonSerializationWriter.propertySeparator);
    };
    this.writeCollectionOfPrimitiveValues = (key, values) => {
      if (!this.shouldWriteValueOrNull(key, values)) {
        return;
      }
      if (values) {
        key && this.writePropertyName(key);
        this.startArray();
        values.forEach((v, idx) => {
          this.writeAnyValue(void 0, v);
          idx + 1 < values.length && this.writer.push(_JsonSerializationWriter.propertySeparator);
        });
        this.endArray();
        key && this.writer.push(_JsonSerializationWriter.propertySeparator);
      }
    };
    this.writeCollectionOfObjectValues = (key, values, serializerMethod) => {
      if (!this.shouldWriteValueOrNull(key, values)) {
        return;
      }
      if (values) {
        key && this.writePropertyName(key);
        this.startArray();
        values.forEach((v) => {
          this.writeObjectValue(void 0, v, serializerMethod);
          this.writer.push(_JsonSerializationWriter.propertySeparator);
        });
        if (values.length > 0) {
          this.writer.pop();
        }
        this.endArray();
        key && this.writer.push(_JsonSerializationWriter.propertySeparator);
      }
    };
    this.startObject = () => {
      this.writer.push(`{`);
    };
    this.endObject = () => {
      this.writer.push(`}`);
    };
    this.startArray = () => {
      this.writer.push(`[`);
    };
    this.endArray = () => {
      this.writer.push(`]`);
    };
    this.removeLastSeparator = () => {
      if (this.writer.length > 0 && this.writer[this.writer.length - 1] === _JsonSerializationWriter.propertySeparator) {
        this.writer.pop();
      }
    };
    this.writeEnumValue = (key, ...values) => {
      if (values.length > 0) {
        const rawValues = values.filter((x) => x !== void 0).map((x) => `${x}`);
        if (rawValues.length > 0) {
          this.writeStringValue(key, rawValues.reduce((x, y) => `${x}, ${y}`));
        }
      }
    };
    this.writeCollectionOfEnumValues = (key, values) => {
      if (values && values.length > 0) {
        const rawValues = values.filter((x) => x !== void 0).map((x) => `${x}`);
        if (rawValues.length === 0) {
          return;
        }
        key && this.writePropertyName(key);
        this.writer.push(JSON.stringify(rawValues));
        key && this.writer.push(_JsonSerializationWriter.propertySeparator);
      }
    };
    this.getSerializedContent = () => {
      return this.convertStringToArrayBuffer(this.writer.join(``));
    };
    this.convertStringToArrayBuffer = (str) => {
      const encoder = new TextEncoder();
      const encodedString = encoder.encode(str);
      return encodedString.buffer;
    };
    this.writeAdditionalData = (additionalData) => {
      if (additionalData === void 0) return;
      for (const key in additionalData) {
        if (Object.prototype.hasOwnProperty.call(additionalData, key)) {
          this.writeAnyValue(key, additionalData[key]);
        }
      }
    };
    this.writeNonParsableObjectValue = (key, value) => {
      if (key) {
        this.writePropertyName(key);
      }
      this.writer.push(JSON.stringify(value), _JsonSerializationWriter.propertySeparator);
    };
    this.writeAnyValue = (key, value) => {
      if (value === void 0) {
        return;
      }
      if (!this.shouldWriteValueOrNull(key, value)) {
        return;
      }
      const valueType = typeof value;
      if (valueType === "boolean") {
        this.writeBooleanValue(key, value);
      } else if (valueType === "string") {
        this.writeStringValue(key, value);
      } else if (value instanceof Date) {
        this.writeDateValue(key, value);
      } else if (value instanceof DateOnly) {
        this.writeDateOnlyValue(key, value);
      } else if (value instanceof TimeOnly) {
        this.writeTimeOnlyValue(key, value);
      } else if (value instanceof Duration) {
        this.writeDurationValue(key, value);
      } else if (valueType === "number") {
        this.writeNumberValue(key, value);
      } else if (Array.isArray(value)) {
        this.writeCollectionOfPrimitiveValues(key, value);
      } else if (valueType === "object") {
        this.writeNonParsableObjectValue(key, value);
      } else {
        throw new Error(`encountered unknown value type during serialization ${valueType}`);
      }
    };
  }
  writeByteArrayValue(key, value) {
    if (!value) {
      throw new Error("value cannot be undefined");
    }
    const b64 = inNodeEnv() ? Buffer.from(value).toString("base64") : btoa(new TextDecoder().decode(value));
    this.writeStringValue(key, b64);
  }
  writeObjectValue(key, value, serializerMethod) {
    if (value === void 0) {
      return;
    }
    if (!this.shouldWriteValueOrNull(key, value)) {
      return;
    }
    if (isUntypedNode(value)) {
      const untypedNode = value;
      if (isUntypedBoolean(untypedNode)) {
        this.writeBooleanValue(key, untypedNode.getValue());
      } else if (isUntypedString(untypedNode)) {
        this.writeStringValue(key, untypedNode.getValue());
      } else if (isUntypedNull(untypedNode)) {
        this.writeNullValue(key);
      } else if (isUntypedNumber(untypedNode)) {
        this.writeNumberValue(key, untypedNode.getValue());
      } else if (isUntypedObject(untypedNode)) {
        const objectValue = untypedNode.getValue();
        if (objectValue === void 0) return;
        if (key) this.writePropertyName(key);
        this.startObject();
        for (const vKey in objectValue) {
          if (Object.prototype.hasOwnProperty.call(objectValue, vKey)) {
            this.writeObjectValue(vKey, objectValue[vKey], serializerMethod);
          }
        }
        this.removeLastSeparator();
        this.endObject();
        if (key) this.writer.push(_JsonSerializationWriter.propertySeparator);
      } else if (isUntypedArray(untypedNode)) {
        if (key) {
          this.writePropertyName(key);
        }
        const arrValue = untypedNode.getValue();
        this.startArray();
        arrValue.forEach((v, idx) => {
          this.writeObjectValue(void 0, v, serializerMethod);
          idx + 1 < arrValue.length && this.writer.push(_JsonSerializationWriter.propertySeparator);
        });
        this.removeLastSeparator();
        this.endArray();
        key && this.writer.push(_JsonSerializationWriter.propertySeparator);
      } else {
        this.writeAnyValue(key, untypedNode.getValue());
      }
      return;
    }
    if (key) this.writePropertyName(key);
    this.onBeforeObjectSerialization && this.onBeforeObjectSerialization(value);
    this.startObject();
    this.onStartObjectSerialization && this.onStartObjectSerialization(value, this);
    serializerMethod && serializerMethod(this, value);
    this.onAfterObjectSerialization && this.onAfterObjectSerialization(value);
    this.removeLastSeparator();
    this.endObject();
    if (key) this.writer.push(_JsonSerializationWriter.propertySeparator);
  }
};
JsonSerializationWriter.propertySeparator = `,`;

// node_modules/@microsoft/kiota-serialization-json/dist/es/src/browser/jsonParseNodeFactory.js
var JsonParseNodeFactory = class {
  getValidContentType() {
    return "application/json";
  }
  getRootParseNode(contentType, content) {
    if (!content) {
      throw new Error("content cannot be undefined of empty");
    } else if (!contentType) {
      throw new Error("content type cannot be undefined or empty");
    } else if (this.getValidContentType() !== contentType) {
      throw new Error(`expected a ${this.getValidContentType()} content type`);
    }
    return new JsonParseNode(this.convertArrayBufferToJson(content));
  }
  convertArrayBufferToJson(content) {
    const decoder = new TextDecoder();
    const contentAsStr = decoder.decode(content);
    return JSON.parse(contentAsStr);
  }
};

// node_modules/@microsoft/kiota-serialization-json/dist/es/src/jsonSerializationWriterFactory.js
var JsonSerializationWriterFactory = class {
  getValidContentType() {
    return "application/json";
  }
  getSerializationWriter(contentType) {
    if (!contentType) {
      throw new Error("content type cannot be undefined or empty");
    } else if (this.getValidContentType() !== contentType) {
      throw new Error(`expected a ${this.getValidContentType()} content type`);
    }
    return new JsonSerializationWriter();
  }
};
export {
  JsonParseNode,
  JsonParseNodeFactory,
  JsonSerializationWriter,
  JsonSerializationWriterFactory
};
//# sourceMappingURL=@microsoft_kiota-serialization-json.js.map
