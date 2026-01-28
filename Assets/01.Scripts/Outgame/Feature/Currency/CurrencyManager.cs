using System;
using UnityEngine;

// 오직 "재화"만 관리하는 클래스입니다.
// 클린 아키텍처에서는 "서비스"라는 이름을 쓴다. (그러나 게임에서는 보통 "매니저"라고 표현한다.)
public class CurrencyManager : MonoBehaviour
{
   public static CurrencyManager Instance;
   // CRUD
   // 재화  관리란: "데이터에 대한 생성 / 조회 / 사용 / 소모 / 이벤트"

   // 재화 데이터
   public double Gold { get; private set; }
   public double Ruby { get; private set; }

   public static event Action OnDataChanged;

   private void Awake()
   {
      Instance = this;
   }
   
   // 1. 재화 추가
   public void AddGold(double amount)
   {
      Debug.Log(Gold);
      Gold += amount;
      
      OnDataChanged?.Invoke();
   }

   public void AddRuby(double amount)
   {
      Ruby += amount;
      
      OnDataChanged?.Invoke();
   }
   
   // 2. 재화 소모
   public bool TrySpendGold(double amount)
   {
      if (Gold >= amount)
      {
         Gold -= amount;
         
         OnDataChanged?.Invoke();

         return true;
      }

      return false;
   }
   
   public bool TrySpendRuby(double amount)
   {
      if (Ruby >= amount)
      {
         Ruby -= amount;
         
         OnDataChanged?.Invoke();

         return true;
      }

      return false;
   }
}
