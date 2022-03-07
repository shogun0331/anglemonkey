//
//  GdprManager.m
//
//
//  Created by soobin on 2018. 05. 29..
//
//

#import "GdprManager.h"


@implementation GdprManager

+ (NSString*) gdprGetDeviceCountry
{
    return [[NSLocale currentLocale] objectForKey:NSLocaleCountryCode];
}

@end

char* GdprMakeStringCopy (const char* string)
{
    if (string == NULL)
        return NULL;
    
    char* res = (char*)malloc(strlen(string) + 1);
    strcpy(res, string);
    return res;
}

extern "C"
{
    const char* _gdprGetDeviceCountry()
    {
        return GdprMakeStringCopy([[GdprManager gdprGetDeviceCountry] UTF8String]);
    }
}
