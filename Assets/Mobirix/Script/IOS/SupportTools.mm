#include "IOSFunc.h"


extern "C"
{
    void _sb_support_openYoutubePage()
    {
        [IOSFunc youtubePage];
    }

    const char* _sb_support_getLanguageCode()
    {
        const char* string = [IOSFunc getLanguage];
        
        char* res = (char*)malloc(strlen(string) + 1);
        strcpy(res, string);
        return res;
    }
    const char* _sb_support_getNationalCode()
    {
        const char* string = [IOSFunc getCountry];
        
        char* res = (char*)malloc(strlen(string) + 1);
        strcpy(res, string);
        return res;
    }
}