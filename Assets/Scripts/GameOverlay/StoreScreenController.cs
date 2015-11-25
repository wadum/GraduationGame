using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class StoreScreenController : MonoBehaviour {

    public GameObject Preorder, PreorderPurchased, SeasonPass, SeasonPassPurchased, ThankYou;
    public int PreorderPrice, SeasonPassPrice, Wallet;
    // 0 = nothing, 1 = Preorder, 2 = Seasonpass
    public static String PurchasedLevels = "PurchasedLevels";
//    public Text Timer;
    public Text WalletText, PreorderText, SeasonPassText;
    bool startup;

	// Use this for initialization
	void Start () {
        startup = true;
        int Purchased = PlayerPrefs.GetInt(PurchasedLevels);
        if (Purchased == 1)
            BuyPreorder();
        else if (Purchased == 2)
            BuySeasonPass();
        WalletText.text = "$ "+ Wallet.ToString();
        PreorderText.text = "$" + PreorderPrice.ToString();
        SeasonPassText.text = "$" + SeasonPassPrice.ToString();
        startup = false;
    }
	
    public void BuyPreorder()
    {
        if (Buy(PreorderPrice))
        {
            TogglePreorder();
            PlayerPrefs.SetInt(PurchasedLevels, 1);
            // We could reduce price of seasonpass?
            // SeasonPassPrice -= PreorderPrice/2;
        }
        else
            OutOfFunds();
    }
    public void BuySeasonPass()
    {
        if (Buy(SeasonPassPrice))
        {
            TogglePreorder();
            ToggleSeasonPass();
            PlayerPrefs.SetInt(PurchasedLevels, 2);
        }
        else OutOfFunds();
    }

    void TogglePreorder()
    {
        Preorder.SetActive(false);
        PreorderPurchased.SetActive(true);
    }

    void ToggleSeasonPass()
    {
        SeasonPass.SetActive(false);
        SeasonPassPurchased.SetActive(true);
    }

    bool Buy(int price)
    {
        if(Wallet >= price)
        {
            Wallet -= price;
            WalletText.text = "$ " + Wallet.ToString();
            if (!startup)
                ThankYou.SetActive(true);
            return true;
        }
        return false;
    }

    void OutOfFunds()
    {

    }

    public void HideThankYou()
    {
        ThankYou.SetActive(false);
    }
}
