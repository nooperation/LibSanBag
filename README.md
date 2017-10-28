[![Build status](https://ci.appveyor.com/api/projects/status/q1s9b8y5n1i2lpy6?svg=true)](https://ci.appveyor.com/project/nooperation/sanbag)
# SanBag
Project Sansar Bag Utilities

# Bag File Format
![image](https://raw.githubusercontent.com/nooperation/SanBag/master/Docs/BagFormat.png)

The majority of bag files follow a fairly straight forward binary format.
* The minimum size is exactly 1024 bytes. Any unused data in that first 1024 bytes is zero filled.
* The file signature is a 4 byte integer at the very start of the file, consisting of the value 0x66
* Linked list structure where each node is a collection of file metadata
  * Filename
  * File timestamp in nanoseconds
  * File length
  * Offset to file contents in bag file

# Compression
![image](https://raw.githubusercontent.com/nooperation/SanBag/master/Docs/Compression.png)

Most assets will be compressed using commercial [Oodle compression](http://www.radgametools.com/oodlecompressors.htm).
* The first byte of an asset determines the compression type
  * 0x01 - File is not compressed. The next three bytes can be discarded
  * 0xF1 - File is compressed. The next two bytes contains the length of the compressed data
  * 0xF2 - File is compressed. The next four bytes contains the length of the compressed data
* The Oodle compression header starts with the byte 0x8C
* Compressed data is followed by a 10 byte footer

## Texture-Resource
Texture-Resource contains a single DDS image.
* Asset versions
  * 9A8D4BBD19B4CD55

## ScriptCompiledBytecodeResource
![image](https://raw.githubusercontent.com/nooperation/SanBag/master/Docs/ScriptCompiledBytecodeResource.png)

ScriptCompiledBytecode-Resource contains a path to the source script along with compiled .Net assembly for the specified script.
* Asset versions
  * C84707DA067146A9
  * E6AC3244F1076F7B
