[![Build status](https://ci.appveyor.com/api/projects/status/q1s9b8y5n1i2lpy6?svg=true)](https://ci.appveyor.com/project/nooperation/sanbag)
# LibSanBag
Project Sansar Bag Utilities

## Requirements
This software does not include all required dependencies. The following list are additional dependencies that need to be acquired and added to the LibSanBag directory:
* `oo2core_7_win64.dll` Can be found in the [warframe](https://www.warframe.com/download) installation directory *\Warframe\Tools\Oodle\x64\final\oo2core_7_win64.dll*

## Referenced libraries
* [SanTools.LibCRN](https://github.com/nooperation/LibCRN)
* [SanTools.LibDDS](https://github.com/nooperation/LibDDS)
* [SanTools.LibFSB](https://github.com/nooperation/LibFSB)
  * [Fmod](https://www.fmod.com/download)
* [SanTools.LibUserPreferences](https://github.com/nooperation/UserPreferencesExplorer)

# Bag File Format
![image](https://raw.githubusercontent.com/nooperation/LibSanBag/master/Docs/BagFormat.png)

The majority of bag files follow a fairly straight forward binary format.
* The minimum size is exactly 1024 bytes. Any unused data in that first 1024 bytes is zero filled.
* The file signature is a 4 byte integer at the very start of the file, consisting of the value 0x66
* Linked list structure where each node is a collection of file metadata
  * Filename
  * File timestamp in nanoseconds
  * File length
  * Offset to file contents in bag file

# Compression
![image](https://raw.githubusercontent.com/nooperation/LibSanBag/master/Docs/Compression.png)

Most assets will be compressed using commercial [Oodle compression](http://www.radgametools.com/oodlecompressors.htm).
* The first byte (N) of an asset determines the compression type. If the signed value of N >= 0 then the file is not compressed and the next three bytes can be discarded.
  * Compressed length type = (N & 0xF3) - 0xF0:
    * 0 - The next byte contains the length of the compressed data
    * 1 - The next two bytes contains the length of the compressed data
    * 2 - The next four bytes contains the length of the compressed data
    * 3 - The next eight bytes contains the length of the compressed data
  * If (N & 0x0C) >= 4 and decompressed size >= 0x40000 then there is an additional P bytes following the compressed data length, where P = (33 + Q) bytes, where Q is 4 * (value of bytes 30-33 of P).
* The Oodle compression header starts with the byte 0x8C
* Compressed data is followed by a 10 byte footer `01 45EF23CD01AB6789 01` (Endian or some other binary check for `01 0123456789ABCDEF 01` ?)

