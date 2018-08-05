using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerPortraitVisual : MonoBehaviour {

    public CharacterAsset charAsset;
    [Header("Text Component References")]
    //public Text NameText;
    public Text HealthText;
    [Header("Image References")]
    public Image HeroPowerIconImage;
    public Image HeroPowerBackgroundImage;
    public Image PortraitImage;
    public Image PortraitBackgroundImage;

    void Awake()
	{
		if(charAsset != null)
			ApplyLookFromAsset();
	}
	
	public void ApplyLookFromAsset()
    {
        HealthText.text = charAsset.MaxHealth.ToString();
        if(HeroPowerIconImage!=null){
        HeroPowerIconImage.sprite = charAsset.HeroPowerIconImage;}

        if(HeroPowerBackgroundImage){
        HeroPowerBackgroundImage.sprite = charAsset.HeroPowerBGImage;
        HeroPowerBackgroundImage.color = charAsset.HeroPowerBGTint;}

        PortraitImage.sprite = charAsset.AvatarImage;
        PortraitBackgroundImage.sprite = charAsset.AvatarBGImage;

        
        PortraitBackgroundImage.color = charAsset.AvatarBGTint;

    }

    public void TakeDamage(int amount, int healthAfter)
    {
        if (amount != 0)
        {
            DamageEffect.CreateDamageEffect(transform.position, amount);
            HealthText.text = System.Math.Min(healthAfter, charAsset.MaxHealth).ToString();
        }
    }

    public void Explode()
    {
        Instantiate(GlobalSettings.Instance.ExplosionPrefab, transform.position, Quaternion.identity);
        Sequence s = DOTween.Sequence();
        s.PrependInterval(2f);
        s.OnComplete(() => GlobalSettings.Instance.GameOverPanel.SetActive(true));
    }

    public void Choose(){
        CharAssetLogic charAssetLogic = GameObject.Find("CharLogic").GetComponent<CharAssetLogic>();
        charAssetLogic.playerChar = charAsset;
    }  



}
