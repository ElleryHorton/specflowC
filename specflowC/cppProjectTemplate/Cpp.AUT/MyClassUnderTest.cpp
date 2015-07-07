#include "MyClassUnderTest.h"


MyClassUnderTest::MyClassUnderTest()
{
}

MyClassUnderTest::~MyClassUnderTest()
{
}

bool MyClassUnderTest::ReturnsTrue()
{
	return true;
}

bool MyClassUnderTest::ReturnsFalse()
{
	return false;
}

int MyClassUnderTest::StringToInt(string value)
{
	return stoi(value);
}

string MyClassUnderTest::IntToString(int value)
{
	return to_string(value);
}