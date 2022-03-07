//
//  IOSFunc.m
//  Maze
//
//  Created by insu cho on 2014. 7. 9..
//
//

#import "IOSFunc.h"

@implementation IOSFunc


static char _country[5];

+ (const char*)getCountry
{
    memset(_country, 0x00, sizeof(_country));
    
    NSString *countryCode = [[NSLocale currentLocale] objectForKey:NSLocaleCountryCode];
    
    const char *tempCountry = [[countryCode lowercaseString] UTF8String];
    
    size_t len = strlen(tempCountry);
    
    if(len > 1)
    {
        _country[0] = tempCountry[0];
        _country[1] = tempCountry[1];
    }
    else
    {
        strcpy(_country, "");
    }
    
    return _country;
}

static char _language[32];

+ (const char*)getLanguage
{
    // reset language char
    memset(_language, 0x00, sizeof(_language));
    
    NSString *language = [[NSLocale preferredLanguages] objectAtIndex:0];
    
    const char *tempLanguage = [[language lowercaseString] UTF8String];
	strcpy(_language, tempLanguage);
    
    return _language;
}


+ (void)youtubePage
{
    // 유투브 어플로 연동하는 URL 스키마
    NSURL* moreGameUrl = [NSURL URLWithString:@"youtube:///user/mobirix1"];
    
    if([[UIApplication sharedApplication] canOpenURL:moreGameUrl])
    {
        [[UIApplication sharedApplication] openURL:moreGameUrl];
    }
    else
    {
        // URL 연결 실패시 ( 유투브 어플이 없는 경우 ) 웹 페이지로 연동 할 URL
        NSURL* moreSapariUrl = [NSURL URLWithString:@"https://www.youtube.com/user/mobirix1"];
        
        // URL 연결
        [[UIApplication sharedApplication] openURL:moreSapariUrl];
    }
}

@end


