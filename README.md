# Crowd Master

<a name="readme-top"></a>
<p>
  Unity 3D 모바일 하이퍼 캐쥬얼 게임 (모작)
</p>

![Image](https://github.com/user-attachments/assets/942bb46c-edb2-4b24-bf0f-b01ffb8b546e)

<br/>

<!-- TABLE OF CONTENTS -->

## 목차

1. [프로젝트 개요](#Intro)
2. [게임 기능](#Features)
3. [게임 플레이](#Play)
4. [핵심 기능](#CoreFeatures)
<br/>

<a name="Intro"></a>
## 프로젝트 개요
- 프로젝트 기간 : 2025.02 ~ 2025.02
- 개발 엔진 및 언어 : Unity & C#
- 플랫폼 : 모바일

<br/>

<a name="Features"></a>
## 게임 기능
1. 화면을 터치하여 좌우로 이동하며 플레이어를 조작합니다.
2. 플레이어가 게이트를 지나면 게이트에 적혀 있는 숫자만큼 군중이 생성됩니다.
3. 맵에 배치된 적이나 장애물과 접촉하면 군중이 줄어듭니다.
4. 게임의 목표는 결승선에 도달하는 것이며, 도달할 때 군중이 얼마나 많은지에 따라 보상이 달라집니다.
<br/>

<a name="Play"></a>
## 게임 플레이
https://youtu.be/8U5QBHiu4mY
<br/>

<a name="CoreFeatures"></a>
## 핵심 기능
1. [군중 생성](https://github.com/haaaabin/Crowd-Master/blob/cabdf07a41be21e853609f03e1f2bb1a27ffffd8/Assets/Scripts/PlayerManager.cs#L235C5-L254C6) / [삭제](https://github.com/haaaabin/Crowd-Master/blob/cabdf07a41be21e853609f03e1f2bb1a27ffffd8/Assets/Scripts/PlayerManager.cs#L304C5-L350C6)
    - 게이트에 적혀 있는 숫자만큼 군중을 생성하고, 군중/적 수 만큼 삭제
    - 새로운 오브젝트를 매번 생성/삭제하는 것이 아니라, 오브젝트 풀링을 사용하여 최적화

2. DOTween을 사용하여 부드럽고 직관적인 애니메이션 적용
    - 텍스트 , 코인 UI , 스틱맨 군중 배열 이동, 점프 효과, 타워 형태로 쌓이는 효과
3. 게임 상태에 따른 동작을 델리게이트로 처리하여 이벤트 기반의 유연한 상태 관리
