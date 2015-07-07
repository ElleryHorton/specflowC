#include "CppUnitTest.h"

#define TEST_CLASS_HOOK_INTEGRATION()						\
BEGIN_TEST_CLASS_ATTRIBUTE()								\
	TEST_CLASS_ATTRIBUTE(L"TestCategory", L"integration")	\
END_TEST_CLASS_ATTRIBUTE()									\

#define TEST_METHOD_HOOK_INTEGRATION(methodName)			\
BEGIN_TEST_METHOD_ATTRIBUTE(methodName)						\
	TEST_METHOD_ATTRIBUTE(L"TestCategory", L"integration")	\
END_TEST_METHOD_ATTRIBUTE()									\
TEST_METHOD(methodName)