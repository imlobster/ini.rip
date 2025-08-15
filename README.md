## <img src="inirip.png" alt="inirip" width="32" height="32"> ini.rip

I really dont know why an ini file will be 1000gb or more, although it can parse it very fast so enjoy it.

## usage

How to use ini.rip:
0. make sure your file is ascii only
1. download INIRIP folder and put it into your sln folder
2. link INIRIP class library to your project
3. extract your .ini file into `string` variable in your script
4. import `using INIRIP;` into your script
5.  use `Coder.TryDecode(yourstring, out Dictionary<ReadOnlyMemory<char>, Dictionary<ReadOnlyMemory<char>, ReadOnlyMemory<char>>> youroutvalues)`
6. then you get your deserialized .ini
7. the structure looks like this:
	* sections
		* keys and values
		* keys and values
	* sections
		* keys and values
		* keys and values
8. to save your values in ini you need to build your `Dictionary<ReadOnlyMemory<char>, Dictionary<ReadOnlyMemory<char>, ReadOnlyMemory<char>>>` or use your edited output from decoding
9. put your full path to file and your dictionary in `Coder.TryEncode("C:\Users\Desktop\MyFile.ini", MyValues)`
10. pray it all to work

## performance on 10k symbols
| Average Time | Median Time | Min Time | Max Time |
|-|-|-|-|
| 44.0 µs        | 44.0 µs       | 26.7 µs      | 66.9 µs    |
######
| Average Symbols/sec | Average MB/sec    |
|-|-|
| 250.83 million       | 478.4 MB/s       |

<sub><sup>Featuring: Water</sup></sub>
