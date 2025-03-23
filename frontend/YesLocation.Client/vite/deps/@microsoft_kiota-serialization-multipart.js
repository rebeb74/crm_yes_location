import {
  MultipartBody
} from "./chunk-K64FEM4K.js";
import "./chunk-TXDUYLVM.js";

// node_modules/@microsoft/kiota-serialization-multipart/dist/es/src/multipartSerializationWriter.js
var MultipartSerializationWriter = class {
  constructor() {
    this.writer = new ArrayBuffer(0);
    this.writeStringValue = (key, value) => {
      if (key) {
        this.writeRawStringValue(key);
      }
      if (value) {
        if (key) {
          this.writeRawStringValue(": ");
        }
        this.writeRawStringValue(value);
      }
    };
    this.writeRawStringValue = (value) => {
      if (value) {
        this.writeByteArrayValue(void 0, new TextEncoder().encode(value).buffer);
      }
    };
    this.writeBooleanValue = (key, value) => {
      throw new Error(`serialization of boolean values is not supported with multipart`);
    };
    this.writeNumberValue = (key, value) => {
      throw new Error(`serialization of number values is not supported with multipart`);
    };
    this.writeGuidValue = (key, value) => {
      throw new Error(`serialization of guid values is not supported with multipart`);
    };
    this.writeDateValue = (key, value) => {
      throw new Error(`serialization of date values is not supported with multipart`);
    };
    this.writeDateOnlyValue = (key, value) => {
      throw new Error(`serialization of date only values is not supported with multipart`);
    };
    this.writeTimeOnlyValue = (key, value) => {
      throw new Error(`serialization of time only values is not supported with multipart`);
    };
    this.writeDurationValue = (key, value) => {
      throw new Error(`serialization of duration values is not supported with multipart`);
    };
    this.writeNullValue = (key) => {
      throw new Error(`serialization of null values is not supported with multipart`);
    };
    this.writeCollectionOfPrimitiveValues = (_key, _values) => {
      throw new Error(`serialization of collections is not supported with multipart`);
    };
    this.writeCollectionOfObjectValues = (_key, _values) => {
      throw new Error(`serialization of collections is not supported with multipart`);
    };
    this.writeObjectValue = (key, value, serializerMethod) => {
      if (!value) {
        throw new Error(`value cannot be undefined`);
      }
      if (!(value instanceof MultipartBody)) {
        throw new Error(`expected MultipartBody instance`);
      }
      if (!serializerMethod) {
        throw new Error(`serializer method cannot be undefined`);
      }
      this.onBeforeObjectSerialization && this.onBeforeObjectSerialization(value);
      this.onStartObjectSerialization && this.onStartObjectSerialization(value, this);
      serializerMethod(this, value);
      this.onAfterObjectSerialization && this.onAfterObjectSerialization(value);
    };
    this.writeEnumValue = (key, ...values) => {
      throw new Error(`serialization of enum values is not supported with multipart`);
    };
    this.writeCollectionOfEnumValues = (key, values) => {
      throw new Error(`serialization of collection of enum values is not supported with multipart`);
    };
    this.getSerializedContent = () => {
      return this.writer;
    };
    this.writeAdditionalData = (additionalData) => {
      throw new Error(`serialization of additional data is not supported with multipart`);
    };
  }
  writeByteArrayValue(key, value) {
    if (!value) {
      throw new Error("value cannot be undefined");
    }
    const previousValue = this.writer;
    this.writer = new ArrayBuffer(previousValue.byteLength + value.byteLength);
    const pipe = new Uint8Array(this.writer);
    pipe.set(new Uint8Array(previousValue), 0);
    pipe.set(new Uint8Array(value), previousValue.byteLength);
  }
};

// node_modules/@microsoft/kiota-serialization-multipart/dist/es/src/multipartSerializationWriterFactory.js
var MultipartSerializationWriterFactory = class {
  getValidContentType() {
    return "multipart/form-data";
  }
  getSerializationWriter(contentType) {
    if (!contentType) {
      throw new Error("content type cannot be undefined or empty");
    } else if (this.getValidContentType() !== contentType) {
      throw new Error(`expected a ${this.getValidContentType()} content type`);
    }
    return new MultipartSerializationWriter();
  }
};
export {
  MultipartSerializationWriter,
  MultipartSerializationWriterFactory
};
//# sourceMappingURL=@microsoft_kiota-serialization-multipart.js.map
