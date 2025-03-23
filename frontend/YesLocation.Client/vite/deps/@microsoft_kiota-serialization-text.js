import {
  DateOnly,
  Duration,
  TimeOnly,
  getEnumValueFromStringValue,
  inNodeEnv,
  parseGuidString
} from "./chunk-K64FEM4K.js";
import "./chunk-TXDUYLVM.js";

// node_modules/@microsoft/kiota-serialization-text/dist/es/src/textParseNode.js
var TextParseNode = class _TextParseNode {
  constructor(text) {
    this.text = text;
    this.getStringValue = () => this.text;
    this.getChildNode = (identifier) => {
      throw new Error(_TextParseNode.noStructuredDataMessage);
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
    this.getNumberValue = () => Number(this.text);
    this.getGuidValue = () => parseGuidString(this.text);
    this.getDateValue = () => new Date(Date.parse(this.text));
    this.getDateOnlyValue = () => DateOnly.parse(this.getStringValue());
    this.getTimeOnlyValue = () => TimeOnly.parse(this.getStringValue());
    this.getDurationValue = () => Duration.parse(this.getStringValue());
    this.getCollectionOfPrimitiveValues = () => {
      throw new Error(_TextParseNode.noStructuredDataMessage);
    };
    this.getCollectionOfEnumValues = (type) => {
      throw new Error(_TextParseNode.noStructuredDataMessage);
    };
    this.getEnumValue = (type) => {
      const rawValue = this.getStringValue();
      if (!rawValue) {
        return void 0;
      }
      return getEnumValueFromStringValue(rawValue, type);
    };
    if (this.text && this.text.length > 1 && this.text.startsWith('"') && this.text.endsWith('"')) {
      this.text = this.text.substring(1, this.text.length - 2);
    }
  }
  getByteArrayValue() {
    const strValue = this.getStringValue();
    if (strValue && strValue.length > 0) {
      return inNodeEnv() ? Buffer.from(strValue, "base64").buffer : new TextEncoder().encode(strValue);
    }
    return void 0;
  }
  /* eslint-disable @typescript-eslint/no-unused-vars */
  getCollectionOfObjectValues(parsableFactory) {
    throw new Error(_TextParseNode.noStructuredDataMessage);
  }
  /* eslint-disable @typescript-eslint/no-unused-vars */
  getObjectValue(parsableFactory) {
    throw new Error(_TextParseNode.noStructuredDataMessage);
  }
};
TextParseNode.noStructuredDataMessage = "text does not support structured data";

// node_modules/@microsoft/kiota-serialization-text/dist/es/src/textSerializationWriter.js
var TextSerializationWriter = class _TextSerializationWriter {
  constructor() {
    this.writer = [];
    this.writeStringValue = (key, value) => {
      if (key || key !== "") {
        throw new Error(_TextSerializationWriter.noStructuredDataMessage);
      }
      if (value !== void 0) {
        if (this.writer.length > 0) {
          throw new Error("a value was already written for this serialization writer, text content only supports a single value");
        } else {
          const isNullValue = value === null;
          this.writer.push(isNullValue ? "null" : value);
        }
      }
    };
    this.writeBooleanValue = (key, value) => {
      if (value !== void 0) {
        this.writeStringValue(key, `${value}`);
      }
    };
    this.writeNumberValue = (key, value) => {
      if (value === null) {
        return this.writeNullValue(key);
      }
      if (value) {
        this.writeStringValue(key, `${value}`);
      }
    };
    this.writeGuidValue = (key, value) => {
      if (value === null) {
        return this.writeNullValue(key);
      }
      if (value) {
        this.writeStringValue(key, `"${value}"`);
      }
    };
    this.writeDateValue = (key, value) => {
      if (value === null) {
        return this.writeNullValue(key);
      }
      if (value) {
        this.writeStringValue(key, `"${value.toISOString()}"`);
      }
    };
    this.writeDateOnlyValue = (key, value) => {
      if (value === null) {
        return this.writeNullValue(key);
      }
      if (value) {
        this.writeStringValue(key, `"${value.toString()}"`);
      }
    };
    this.writeTimeOnlyValue = (key, value) => {
      if (value === null) {
        return this.writeNullValue(key);
      }
      if (value) {
        this.writeStringValue(key, `"${value.toString()}"`);
      }
    };
    this.writeDurationValue = (key, value) => {
      if (value === null) {
        return this.writeNullValue(key);
      }
      if (value) {
        this.writeStringValue(key, `"${value.toString()}"`);
      }
    };
    this.writeNullValue = (key) => {
      this.writeStringValue(key, `null`);
    };
    this.writeCollectionOfPrimitiveValues = (key, values) => {
      throw new Error(_TextSerializationWriter.noStructuredDataMessage);
    };
    this.writeCollectionOfObjectValues = (key, values, serializerMethod) => {
      throw new Error(_TextSerializationWriter.noStructuredDataMessage);
    };
    this.writeObjectValue = (key, value, serializerMethod) => {
      throw new Error(_TextSerializationWriter.noStructuredDataMessage);
    };
    this.writeEnumValue = (key, ...values) => {
      if (values.length > 0) {
        const rawValues = values.filter((x) => x !== void 0).map((x) => `${x}`);
        if (rawValues.length > 0) {
          this.writeStringValue(key, rawValues.reduce((x, y) => `${x},${y}`));
        }
      }
    };
    this.writeCollectionOfEnumValues = (key, values) => {
      this.writeEnumValue(key, values);
    };
    this.getSerializedContent = () => {
      return this.convertStringToArrayBuffer(this.writer.join(``));
    };
    this.convertStringToArrayBuffer = (str) => {
      const encoder = new TextEncoder();
      const encodedString = encoder.encode(str);
      return encodedString.buffer;
    };
    this.writeAdditionalData = (value) => {
      throw new Error(_TextSerializationWriter.noStructuredDataMessage);
    };
  }
  writeByteArrayValue(key, value) {
    if (!value) {
      throw new Error("value cannot be undefined");
    }
    const b64 = inNodeEnv() ? Buffer.from(value).toString("base64") : btoa(new TextDecoder().decode(value));
    this.writeStringValue(key, b64);
  }
};
TextSerializationWriter.noStructuredDataMessage = "text does not support structured data";

// node_modules/@microsoft/kiota-serialization-text/dist/es/src/browser/textParseNodeFactory.js
var TextParseNodeFactory = class {
  getValidContentType() {
    return "text/plain";
  }
  getRootParseNode(contentType, content) {
    if (!content) {
      throw new Error("content cannot be undefined of empty");
    } else if (!contentType) {
      throw new Error("content type cannot be undefined or empty");
    } else if (this.getValidContentType() !== contentType) {
      throw new Error(`expected a ${this.getValidContentType()} content type`);
    }
    return new TextParseNode(this.convertArrayBufferToText(content));
  }
  convertArrayBufferToText(arrayBuffer) {
    const decoder = new TextDecoder();
    return decoder.decode(arrayBuffer);
  }
};

// node_modules/@microsoft/kiota-serialization-text/dist/es/src/textSerializationWriterFactory.js
var TextSerializationWriterFactory = class {
  getValidContentType() {
    return "text/plain";
  }
  getSerializationWriter(contentType) {
    if (!contentType) {
      throw new Error("content type cannot be undefined or empty");
    } else if (this.getValidContentType() !== contentType) {
      throw new Error(`expected a ${this.getValidContentType()} content type`);
    }
    return new TextSerializationWriter();
  }
};
export {
  TextParseNode,
  TextParseNodeFactory,
  TextSerializationWriter,
  TextSerializationWriterFactory
};
//# sourceMappingURL=@microsoft_kiota-serialization-text.js.map
