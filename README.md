# WORK ON SERIALIZER IN PROGRESS!

## <img src="inirip.png" alt="inirip" width="32" height="32"> ini.rip

I really dont know why th are you going to use this, although it works well so enjoy it till its working. Dont take it too seriously.

## usage

To use ini.rip you need to:
1. download INIRIP folder and put it into your sln folder
2. link INIRIP class library to your project
3. extract your .ini file into `string` variable in your script
4. import `using INIRIP;` into your script
5.  use `INIRIP.Encoder.TryDecode(yourstring, out Dictionary<ReadOnlyMemory<char>, Dictionary<ReadOnlyMemory<char>, ReadOnlyMemory<char>>> youroutvalues)`
6. then you get your deserialized .ini
7. the structure looks like this:
	* sections
		* keys and values
		* keys and values
	* sections
		* keys and values
		* keys and values
8. pray it to work

## performance
| Average Time   | Median Time   | Min Time     | Max Time   |
|-|-|-|-|
| 44.0 µs        | 44.0 µs       | 26.7 µs      | 66.9 µs    |
######
| Average Symbols/sec | Average MB/sec    |
|-|-|
| 250.83 million       | 478.4 MB/s       |

<sub><sup>Featuring: Water</sup></sub>
