#include <string>

using namespace std;

class MyClassUnderTest
{
public:
	MyClassUnderTest();
	~MyClassUnderTest();

	bool ReturnsTrue();
	bool ReturnsFalse();
	int StringToInt(string value);
	string IntToString(int value);
};

