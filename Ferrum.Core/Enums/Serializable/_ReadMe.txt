A serializer will be created for each Enum type in this folder.

The Ferrum.Core.Extensions.AddEnumSerializers extension method will create a default Json serializers.

The default serializer will read and write the string equivalent to/from Json instead of the underlying numeric type.

When reading from Json if the string type is not found the default value for the enum is returned.
