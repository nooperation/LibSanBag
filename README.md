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
