#include <string>

namespace {
	void trim_left_if(std::string &s, std::string delimiter)
	{
		s.erase(0, s.find_first_not_of(delimiter));
	}

	void trim_right_if(std::string &s, std::string delimiter)
	{
		if (s.find_last_not_of(delimiter) == s.npos) {
			// the string may only contain the delimiter to remove so remove it
			s.erase(0, s.find_first_not_of(delimiter));
		}
		else {
			s.erase(s.find_last_not_of(delimiter) + 1, s.length() - 1);
		}
	}
}