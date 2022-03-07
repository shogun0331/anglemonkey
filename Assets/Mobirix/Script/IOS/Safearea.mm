extern "C"
{
    const char* _sb_safearea_getScreenScale()
    {
        float scaleX = 1;
        float scaleY = 1;
        
        // Safearea 스케일 값 처리
        if (@available(iOS 11.0, *)) {
            // 단말기의 크기
            CGRect screenSize = [[UIScreen mainScreen] bounds];
            // safearea 의 크기
            UIEdgeInsets insets = [UnityGetMainWindow() safeAreaInsets];
            
            
            // 상단기준 스케일 사이즈
            float scaleTop = 1 - (insets.top * 2 / screenSize.size.height);
            
            // 하단기준 스케일 사이즈
            float scaleBottom = 1 - (insets.bottom * 2 / screenSize.size.height);
            
            // 왼쪽 기준 스케일 사이즈
            float scaleLeft = 1 - (insets.left * 2 / screenSize.size.width);
            
            // 오른쪽 기준 스케일 사이즈
            float scaleRight = 1 - (insets.right * 2 / screenSize.size.width);
            
            // 더 작은 스케일을 선택
            scaleX = scaleLeft < scaleRight ? scaleLeft : scaleRight;
            scaleY = scaleTop < scaleBottom ? scaleTop : scaleBottom;
            
        } else {
            // iOS 11 이전 버전은 스케일 조절을 하지 않음
            
        }
        // Safearea 스케일 값 처리 끝
        
        char scale[256] = {0x00, };
        sprintf(scale, "%f|%f", scaleX, scaleY);
        
        char* res = (char*)malloc(strlen(scale) + 1);
        strcpy(res, scale);
        
        return res;
    }
}
