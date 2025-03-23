import {
  DateOnly,
  Duration,
  TimeOnly,
  createBackedModelProxyHandler,
  getEnumValueFromStringValue,
  isBackingStoreEnabled,
  parseGuidString
} from "./chunk-K64FEM4K.js";
import "./chunk-TXDUYLVM.js";

// node_modules/@microsoft/kiota-serialization-form/dist/es/src/formParseNode.js
var FormParseNode = class _FormParseNode {
  /**
   *  Creates a new instance of FormParseNode
   * @param _rawString the raw string to parse
   */
  constructor(_rawString) {
    this._rawString = _rawString;
    this._fields = {};
    this.normalizeKey = (key) => decodeURIComponent(key).trim();
    this.getStringValue = () => decodeURIComponent(this._rawString);
    this.getChildNode = (identifier) => {
      if (this._fields[identifier]) {
        return new _FormParseNode(this._fields[identifier]);
      }
      return void 0;
    };
    this.getBooleanValue = () => {
      var _a;
      const value = (_a = this.getStringValue()) === null || _a === void 0 ? void 0 : _a.toLowerCase();
      if (value === "true" || value === "1") {
        return true;
      } else if (value === "false" || value === "0") {
        return false;
      }
      return void 0;
    };
    this.getNumberValue = () => parseFloat(this.getStringValue());
    this.getGuidValue = () => parseGuidString(this.getStringValue());
    this.getDateValue = () => new Date(Date.parse(this.getStringValue()));
    this.getDateOnlyValue = () => DateOnly.parse(this.getStringValue());
    this.getTimeOnlyValue = () => TimeOnly.parse(this.getStringValue());
    this.getDurationValue = () => Duration.parse(this.getStringValue());
    this.getCollectionOfPrimitiveValues = () => {
      return this._rawString.split(",").map((x) => {
        const currentParseNode = new _FormParseNode(x);
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
    this.getCollectionOfObjectValues = (parsableFactory) => {
      throw new Error(`serialization of collections is not supported with URI encoding`);
    };
    this.getObjectValue = (parsableFactory) => {
      const temp = {};
      const enableBackingStore = isBackingStoreEnabled(parsableFactory(this)(temp));
      const value = enableBackingStore ? new Proxy(temp, createBackedModelProxyHandler()) : temp;
      if (this.onBeforeAssignFieldValues) {
        this.onBeforeAssignFieldValues(value);
      }
      this.assignFieldValues(value, parsableFactory);
      if (this.onAfterAssignFieldValues) {
        this.onAfterAssignFieldValues(value);
      }
      return value;
    };
    this.getCollectionOfEnumValues = (type) => {
      const rawValues = this.getStringValue();
      if (!rawValues) {
        return [];
      }
      return rawValues.split(",").map((x) => getEnumValueFromStringValue(x, type));
    };
    this.getEnumValue = (type) => {
      const rawValue = this.getStringValue();
      if (!rawValue) {
        return void 0;
      }
      return getEnumValueFromStringValue(rawValue, type);
    };
    this.assignFieldValues = (model, parsableFactory) => {
      const fields = parsableFactory(this)(model);
      Object.entries(this._fields).filter((x) => !/^null$/i.test(x[1])).forEach(([k, v]) => {
        const deserializer = fields[k];
        if (deserializer) {
          deserializer(new _FormParseNode(v));
        } else {
          model[k] = v;
        }
      });
    };
    if (!_rawString) {
      throw new Error("rawString cannot be undefined");
    }
    _rawString.split("&").map((x) => x.split("=")).filter((x) => x.length === 2).forEach((x) => {
      const key = this.normalizeKey(x[0]);
      if (this._fields[key]) {
        this._fields[key] += "," + x[1];
      } else {
        this._fields[key] = x[1];
      }
    });
  }
  getByteArrayValue() {
    throw new Error("serialization of byt arrays is not supported with URI encoding");
  }
};

// node_modules/@microsoft/kiota-serialization-form/dist/es/src/formSerializationWriter.js
var FormSerializationWriter = class _FormSerializationWriter {
  constructor() {
    this.writer = [];
    this.depth = -1;
    this.writeStringValue = (key, value) => {
      if (value === null) {
        value = "null";
      }
      if (key && value) {
        this.writePropertyName(key);
        this.writer.push(`=${encodeURIComponent(value)}`);
        this.writer.push(_FormSerializationWriter.propertySeparator);
      }
    };
    this.writePropertyName = (key) => {
      this.writer.push(encodeURIComponent(key));
    };
    this.shouldWriteValueOrNull = (key, value) => {
      if (value === null) {
        this.writeNullValue(key);
        return false;
      }
      return true;
    };
    this.writeBooleanValue = (key, value) => {
      if (this.shouldWriteValueOrNull(key, value)) {
        value !== void 0 && this.writeStringValue(key, `${value}`);
      }
    };
    this.writeNumberValue = (key, value) => {
      if (this.shouldWriteValueOrNull(key, value)) {
        value && this.writeStringValue(key, `${value}`);
      }
    };
    this.writeGuidValue = (key, value) => {
      if (this.shouldWriteValueOrNull(key, value)) {
        value && this.writeStringValue(key, value.toString());
      }
    };
    this.writeDateValue = (key, value) => {
      if (this.shouldWriteValueOrNull(key, value)) {
        value && this.writeStringValue(key, value.toISOString());
      }
    };
    this.writeDateOnlyValue = (key, value) => {
      if (this.shouldWriteValueOrNull(key, value)) {
        value && this.writeStringValue(key, value.toString());
      }
    };
    this.writeTimeOnlyValue = (key, value) => {
      if (this.shouldWriteValueOrNull(key, value)) {
        value && this.writeStringValue(key, value.toString());
      }
    };
    this.writeDurationValue = (key, value) => {
      if (this.shouldWriteValueOrNull(key, value)) {
        value && this.writeStringValue(key, value.toString());
      }
    };
    this.writeNullValue = (key) => {
      key && this.writeStringValue(key, null);
    };
    this.writeCollectionOfPrimitiveValues = (_key, _values) => {
      if (_key && _values) {
        _values.forEach((val) => {
          this.writeAnyValue(_key, val);
        });
      }
    };
    this.writeCollectionOfObjectValues = (_key, _values) => {
      throw new Error(`serialization of collections is not supported with URI encoding`);
    };
    this.writeObjectValue = (key, value, serializerMethod) => {
      if (++this.depth > 0) {
        throw new Error(`serialization of nested objects is not supported with URI encoding`);
      }
      if (!this.shouldWriteValueOrNull(key, value)) {
        return;
      }
      if (value) {
        if (key) {
          this.writePropertyName(key);
        }
        this.onBeforeObjectSerialization && this.onBeforeObjectSerialization(value);
        this.onStartObjectSerialization && this.onStartObjectSerialization(value, this);
        serializerMethod(this, value);
        this.onAfterObjectSerialization && this.onAfterObjectSerialization(value);
        if (this.writer.length > 0 && this.writer[this.writer.length - 1] === _FormSerializationWriter.propertySeparator) {
          this.writer.pop();
        }
        key && this.writer.push(_FormSerializationWriter.propertySeparator);
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
      if (key && values && values.length > 0) {
        const rawValues = values.filter((x) => x !== void 0).map((x) => `${x}`);
        if (rawValues.length > 0) {
          this.writeCollectionOfPrimitiveValues(key, rawValues);
        }
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
        this.writeAnyValue(key, additionalData[key]);
      }
    };
    this.writeAnyValue = (key, value) => {
      if (value === null) {
        return this.writeNullValue(key);
      }
      if (value !== void 0) {
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
        } else {
          throw new Error(`encountered unknown ${value} value type during serialization ${valueType} for key ${key}`);
        }
      }
    };
  }
  writeByteArrayValue(key, value) {
    throw new Error("serialization of byt arrays is not supported with URI encoding");
  }
};
FormSerializationWriter.propertySeparator = `&`;

// node_modules/@microsoft/kiota-serialization-form/dist/es/src/browser/formParseNodeFactory.js
var FormParseNodeFactory = class {
  getValidContentType() {
    return "application/x-www-form-urlencoded";
  }
  getRootParseNode(contentType, content) {
    if (!content) {
      throw new Error("content cannot be undefined of empty");
    } else if (!contentType) {
      throw new Error("content type cannot be undefined or empty");
    } else if (this.getValidContentType() !== contentType) {
      throw new Error(`expected a ${this.getValidContentType()} content type`);
    }
    return new FormParseNode(this.convertArrayBufferToString(content));
  }
  convertArrayBufferToString(content) {
    const decoder = new TextDecoder();
    return decoder.decode(content);
  }
};

// node_modules/@microsoft/kiota-serialization-form/dist/es/src/formSerializationWriterFactory.js
var FormSerializationWriterFactory = class {
  getValidContentType() {
    return "application/x-www-form-urlencoded";
  }
  getSerializationWriter(contentType) {
    if (!contentType) {
      throw new Error("content type cannot be undefined or empty");
    } else if (this.getValidContentType() !== contentType) {
      throw new Error(`expected a ${this.getValidContentType()} content type`);
    }
    return new FormSerializationWriter();
  }
};
export {
  FormParseNode,
  FormParseNodeFactory,
  FormSerializationWriter,
  FormSerializationWriterFactory
};
//# sourceMappingURL=@microsoft_kiota-serialization-form.js.map
