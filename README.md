
# Skillz 개발 이슈
![enter image description here](https://docs.skillz.com/assets/images/logo-6e4ad7f189cfa706b043c8610c3ffeed.png)
 최신 Unity Skillz SDK(ver27)를 기준으로 작성된 문서입니다.
 

>[Skillz Developer  Documentation](https://docs.skillz.com/docs/welcome/)
>작성일 : 2022.01.19
>작성자 : 모비릭스 개발팀 김정환

## 목차 
 >[**연동 개발 플로우**](#연동-개발-플로우)
 >[**중단된 경기**](#중단된-경기)
 >[**부정 행위 방지 기술**](#부정-행위-방지-기술)
 >[**튜토리얼 가이드 정책**](#튜토리얼-가이드-정책)
 >[**동점 처리를 막기 위한 게임 개발 이슈**](#동점-처리를-막기-위한-게임-개발-이슈)
 >[**기타 개발 주요 이슈 정리**](#기타-개발-주요-이슈-정리)


## 연동 개발 플로우
### SkillzFlow
![enter image description here](https://docs.skillz.com/assets/images/unity-lifecycle-2cc87d567ff7c30e2146d835e46564f6.png)


### Match
> Skillz UI에서 제공 되는 인터페이스
> > **OnMatchWillBegin**<br> - Skillz UI에서 Match가 완료 되었을때 
> >  **OnProgressionRoomEnter**<br> -  Skillz UI에서 진행률 진입점 또는 사이드 메뉴를 클릭하면 호출됩니다
> > **OnSkillzWillExit** <br> -   Skillz UI에서 사이드 메뉴에서 종료를 선택하면 호출됩니다.
- 예제 스크립트
 ```cs
using SkillzSDK;

public sealed class SkillzGameController : SkillzMatchDelegate
{
    // Called when a player chooses a tournament and the match countdown expires
    public void OnMatchWillBegin(Match matchInfo) {
    }

    // Called when a player clicks the Progression entry point or side menu. Explained in later steps
    public void OnProgressionRoomEnter() {

    }

    // Called when a player chooses Exit Skillz from the side menu
    public void OnSkillzWillExit() {

    }
}

```
[참고 사이트](https://docs.skillz.com/docs/launch-skillz-ui)

### GamePlay
> Skillz 게임에 제공되는 함수
> > **SubmitScore**<br> - Skillz에  스코어를 보고 합니다.
> >  **ReturnToSkillz**<br> -  Skillz UI로 돌아갑니다.


- 예제 스크립트
 ```cs
using Skillz;
using UnityEngine;

public class MatchController : MonoBehaviour
{
    void TryToSubmitScore() {
        string score = GetScore();
        SkillzCrossPlatform.SubmitScore(score, OnSuccess, OnFailure);
    }

    void OnSuccess() {
        Debug.Log("Success");
        MatchComplete();
    }

    void OnFailure(string reason) {
        Debug.LogWarning("Fail: " + reason);
        StartCoroutine(RetrySubmit());
    }

    IEnumerator RetrySubmit() {
        yield return new WaitForSeconds(_retrySeconds);
        TryToSubmitScore();
    }

    void MatchComplete(){
        SkillzCrossPlatform.ReturnToSkillz();
    }

}
  ```
[참고 사이트](https://docs.skillz.com/docs/play-and-compare-gameplay/)

## 중단된 경기

### 제출된 점수가 없는 경우 
-   ***의도하지 않은 충돌*** - 게임 중 앱 충돌 또는 하드웨어 오류
-   ***백그라운드*** - 앱이 백그라운드됨(OS가 앱을 종료하는 경우에도 백그라운드로 간주됨)
-   ***종료됨*** - 사용자가 수동으로 앱을 종료합니다.
-   ***타임아웃*** - 사용자가 할당된 시간 내에 점수를 제출하지 않았습니다. 또한 다른 정보를 사용할 수 없는 경우 기본 중단 유형이 기록됩니다.

게임 클라이언트가 처리해야 하는 점수를 제대로 제출하지 않은 플레이어를 처리하는 다른 두 가지 중요한 방법이 있습니다. **게임 내 몰수 및** **중단 메서드** 호출 ("의도적" 중단이라고 함)입니다. 게임 내 몰수를 처리하는 방법은 **고유한 게임 규칙**에 따라 다르지만 고려해야 할 몇 가지 일반적인 항목이 있습니다.

[참고 사이트](https://docs.skillz.com/docs/aborts)

## 부정 행위 방지 기술

>**부정 행위**를 위한 한 가지 기술은 런타임 메모리의 값을 수정하는 것입니다. 이 기술을 사용하면 사용자가 Skillz에 제출하기 전에 점수를 수정하거나 건강이나 시간과 같은 게임 플레이 변수를 조작하여 부당한 이점을 얻을 수 있습니다.

### 데이터 복제 및 확인 
>메모리가 수정되지 않도록 보호하는 한 가지 기술은 데이터를 복제하고 확인하는 것입니다. 
>아이디어는 주요 데이터를 별도의 변수에 복사한 다음 복사본을 원본과 비교하는 것입니다. 
>두 게임이 게임의 어느 지점에서든 일치하지 않으면 누군가 부정 행위를 하고 있을 가능성이 있습니다.

이것은 치트를 위해 수정되어야 하는 메모리에 최소한 두 곳이 있음을 보장합니다.
- 예제 스크립트
```
private int score = 0;
private int scoreCheck = 0;

// Make sure score and scoreCheck start at the same number
void Awake()
{
    scoreCheck = score;
}

// Any code that alters the score should also alter the scoreCheck
private void AddScore(int increase)
{
    score += increase;
    scoreCheck += increase;
}

// Accessors just need to return the real score
private int GetScore ()
{
    return score;
}

// Assume this is your in-game function triggered upon
// finishing the game (eg: time runs out, no more moves)
private void GameOver()
{
    if (ConfirmScoreValidity())
    {
        // The score matches the copy, so report it
        SkillzCrossPlatform.ReportFinalScore(score);
    }
    else
    {
        // Abort the match because suspicious behavior was detected
        SkillzCrossPlatform.AbortMatch();
    }
}

// Confirms that the score is valid.
private bool ConfirmScoreValidity()
{
    return score == scoreCheck;
}
```

### 난독화 

>메모리가 수정되지 않도록 보호하는 또 다른 기술은 데이터를 난독화하는 것입니다.
> 다양한 수단을 통해 키 데이터를 난독화하거나 암호화할 수 있습니다. 
> 메모리에 쓰기 전에 변수를 난독화하거나 암호화하십시오. 
> 간단한 XOR에서 더 복잡한 암호화 방법에 이르기까지 이를 달성하기 위한 다양한 기술이 있습니다. 
> 이것을 복제 기술과 결합하는 경우 복제를 해시하고 해시 검사를 확인 논리에 통합할 수도 있습니다.
> 데이터를 난독화하면 치터가 메모리에서 주요 변수를 찾기가 더 어려워집니다.

```
private int xorCode = 12345; // USE A DIFFERENT XOR VALUE THAN THIS!
private int score = 0;

// Make sure to xor the initial score
void Awake() {
    score = score ^ xorCode;
}

// Functions and mutators should xor the value before using it, and xor
// the value again before the function exits
private void AddScore(int increase) {
    score ^= xorCode;
    score += increase;
    score ^= xorCode;
}

// Accessors just need to return the xor'd value
private int GetScore () {
    return score ^ xorCode;
}
```

### 고급 데이터 보호
>추가 보안을 위해 데이터 복제 및 난독화 기술을 결합할 수 있습니다. 
>이상적으로는 score 및 scoreCheck 값에 대해 다른 XOR 해시를 사용해야 합니다.
> 그렇게 하면 사기꾼이 알아채기가 더 어려워집니다. 
> 그렇지 않으면 메모리 주소에 일치하는 값이 포함되어 체크섬을 더 쉽게 찾을 수 있습니다.
```
private  int xorCode =  12345;  // USE A DIFFERENT XOR VALUE THAN THIS!  
private  int scoreCheckXorCode =  54321;  // USE A DIFFERENT XOR VALUE THAN THIS!  
private  int score =  0;  
private  int scoreCheck =  0;  
  
// Make sure to xor the initial values  
@Override  
private  void  onCreate(Bundle savedInstanceState)  {  
score = score ^ xorCode;  
scoreCheck = scoreCheck ^ scoreCheckXorCode;  
}  
  
// Functions and mutators should xor the value before using it, and xor the value again before the function exits  
private  void  addScore(int increase)  {  
score ^= xorCode;  
score += increase;  
score ^= xorCode;  
scoreCheck ^= scoreCheckXorCode;  
scoreCheck += increase;  
scoreCheck ^= scoreCheckXorCode;  
}  
  
// Confirms that the score is valid.  
private  boolean  confirmScoreValidity()  {  
return  (score ^ xorCode)  ==  (scoreCheck ^ scoreCheckXorCode);  
}
```


[참고 사이트](https://docs.skillz.com/docs/anti-cheating-techniques-overview)

## 튜토리얼 가이드 정책

### 슬라이드 튜토리얼 
![enter image description here](https://docs.skillz.com/assets/images/sol-cube-56ca35eb15cad2f73a59d537ce3547fc.png)
-   최상의 전체 성능을 위해 강력히 권장됩니다. 이 형식은 마찰을 최소화하면서 많은 귀중한 정보를 제공할 수 있습니다.
-   각 슬라이드의 명확한 테마 또는 개념을 통해 플레이어는 경기 중에도 원하는 특정 정보를 쉽게 찾을 수 있습니다.
-   게임에 여러 역학 또는 구조화된 라운드 순서가 있는 경우 권장됩니다.
-   6페이지 미만으로 제한될 때 가장 좋습니다.
-   애니메이션 시각 자료는 주요 개념을 명확히 하고 이해도를 높이며 게임에 보다 세련된 느낌을 주는 데 도움이 되므로 강력히 권장됩니다.


### 인포그래픽 튜토리얼 
![enter image description here](https://docs.skillz.com/assets/images/tutorial-example-c6a5ac801a7234898c34849d3106e37a.png)
-   하나의 슬라이드에서 설명할 수 있는 간단한 역학을 가진 게임에 적합합니다.
-   가능한 경우 텍스트보다 이미지와 다이어그램에 중점을 둡니다.
-   여러 게임 모드가 있는 게임에 적극 권장됩니다.

1.  예를 들어, 게임에 레벨에 대한 특정 또는 고유한 목표 또는 장애물이 있는 경우 앞으로 있을 일에 대한 입문서로 해당 역학에 특별히 초점을 맞춘 "매치 시작" 인포그래픽 화면을 표시하는 것이 중요합니다.
2.  이 보다 구체적인 "경기 시작" 인포그래픽은 FTUE의 일부로 표시되는 더 넓은 일회성 인포그래픽과 다릅니다.

-   일반적인 FTUE 인포그래픽은 첫 번째 게임에서 한 번 나타납니다. "경기 시작" 인포그래픽은 임시 정비사에 대한 구체적인 설명이 필요한 모든 경기가 시작될 때 나타날 수 있습니다.
-   이 형식은 게임의 새로운 사용자 유입경로에서 마찰을 최소화합니다.
### 실습형 튜토리얼
![enter image description here](https://docs.skillz.com/assets/images/jb-47fcf3f98eaa0502b635e6247f05d3ba.png)
-   체크리스트와 같이 개념을 이해하기 위해 확인해야 하는 상당한 학습 곡선이 있는 게임에 권장됩니다.
-   5개 미만의 상호 작용으로 제한될 때 가장 좋습니다.
-   대화형 자습서는 매우 효과적이지만 마찰이 높으며 항상 건너뛰기 옵션을 포함해야 합니다.

### 비디오 튜토리얼 
![enter image description here](https://docs.skillz.com/assets/images/bowl-7d0d831f3b7067365a4fd21acd512352.png)
-   키네틱 게임 플레이와 실행하기 어려운 시나리오를 보여주는 데 적극 권장됩니다.
-   슬라이드쇼, 인포그래픽 및 대화형 접근 방식이 미묘하거나 상세하지 않을 때 사용합니다.
-   비디오에 버퍼 시간이 필요하지 않고 길이가 40초 미만으로 제한될 때 가장 좋습니다.

[참고 사이트](https://docs.skillz.com/docs/tutorials)


## 동점 처리를 막기 위한 게임 개발 이슈

### 자료 조사 중

[참고 사이트](https://skillz.zendesk.com/hc/en-us/articles/210790463-Tiebreakers)

## 기타 개발 주요 이슈 정리


### 게임 규칙 & 룰 설명

- 승리 조건의 대한 설명 가이드
- 스코어 획득 방법 가이드
- 게임 시간 타이머 필요


