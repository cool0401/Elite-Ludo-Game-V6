using AssemblyCSharp;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class WithdrawDepositLogic : MonoBehaviour
{
    public GameObject netBalance;
    public GameObject withdrawBtn;
    public GameObject depositBtn;
    public Text titleText;

    public GameObject ammountToAddInput;
    public GameObject lastFourDigitInput;
    public GameObject bankImage;
    public GameObject selectedBankAddress;
    public GameObject ammountCoin;
    public GameObject BankOptionsGroup;
    public GameObject SubmitButton;
    public GameObject WithdrawDetailWindow;

    public GameObject SaveButton;
    public GameObject CloseButton;
    public GameObject IFSC;
    public GameObject ID;
    public GameObject FullName;
    public GameObject UPI;
    public GameObject PayTM;
    public GameObject BankAc;
    public GameObject MiniText;
    public GameObject NetBalance;
    public GameObject Ammount;
    public GameObject MobileNumberWithdraw;
    public GameObject ReferenceIDInput;

    public GameObject AmmountToWithdraw;
    public GameObject BankOptionsGroupWithdraw;
    public GameObject SubmitWithdraw;

    public Sprite BkashImage;
    public Sprite NagadImage;

    public string NagadAccountNumber;
    public string BkashAccountNumber;

    public string SelectedBank = "Nagad";

    public string SelectedBankWithdraw = "Nagad";

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetBanksInfo());
    }

    IEnumerator GetBanksInfo()
    {
        using (UnityWebRequest getBankNumbersRequest = UnityWebRequest.Get(StaticStrings.baseURL + "api/banks-numbers/get"))
        {
            yield return getBankNumbersRequest.SendWebRequest();
            BankRootResponse response = JsonUtility.FromJson<BankRootResponse>("{\"banks\":" + getBankNumbersRequest.downloadHandler.text + "}");

            NagadAccountNumber = response.banks.Where(x => x.bank.ToLower() == "nagad").First().bankNumber;
            BkashAccountNumber = response.banks.Where(x => x.bank.ToLower() == "bkash").First().bankNumber;

            selectedBankAddress.GetComponent<Text>().text = NagadAccountNumber;
        }
    }

    public void OnNagadOptionChanged(bool enabled)
    {
        selectedBankAddress.GetComponent<Text>().text = NagadAccountNumber;
        bankImage.GetComponent<Image>().sprite = NagadImage;
        SelectedBank = "Nagad";
    }

    public void OnBkashOptionChanged(bool enabled)
    {
        selectedBankAddress.GetComponent<Text>().text = BkashAccountNumber;
        bankImage.GetComponent<Image>().sprite = BkashImage;
        SelectedBank = "Bkash";
    }

    public void OnNagadWithdrawOptionChanged(bool enabled)
    {
        bankImage.GetComponent<Image>().sprite = NagadImage;
        SelectedBankWithdraw = "Nagad";
    }

    public void OnBkashWithdrawOptionsChanged(bool enabled)
    {
        bankImage.GetComponent<Image>().sprite = BkashImage;
        SelectedBankWithdraw = "Bkash";
    }

    public void OnSubmitClicked()
    {
        StartCoroutine(SubmitDeposit());
    }

    IEnumerator SubmitDeposit()
    {
        SubmitButton.SetActive(false);
        string ammountToAdd = ammountToAddInput.GetComponent<InputField>().text;
        string last4Digits = lastFourDigitInput.GetComponent<InputField>().text;
        string referenceId = ReferenceIDInput.GetComponent<InputField>().text;

        if (!string.IsNullOrWhiteSpace(ammountToAdd) && !string.IsNullOrWhiteSpace(last4Digits) && !string.IsNullOrWhiteSpace(referenceId))
        {
            Dictionary<string, string> wwwForm = new Dictionary<string, string>();
            wwwForm.Add("amountToAdd", ammountToAdd);
            wwwForm.Add("last4Digits", last4Digits);
            wwwForm.Add("bank", SelectedBank);
            wwwForm.Add("referenceId", referenceId);
            wwwForm.Add("PID", PlayerPrefs.GetString("PID"));

            using (UnityWebRequest request = UnityWebRequest.Post(StaticStrings.baseURL + "api/submit-deposit", wwwForm))
            {
                yield return request.SendWebRequest();
                if (request.downloadHandler.text == "success")
                {
                    close();
                    _ShowAndroidToastMessage("Deposit request sent successfully");
                }
            }
        }
        SubmitButton.SetActive(true);
    }

    private void _ShowAndroidToastMessage(string message)
    {
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        if (unityActivity != null)
        {
            AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
            unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity, message, 0);
                toastObject.Call("show");
            }));
        }
    }


    public void withdraw()
    {
        netBalance.SetActive(false);

        withdrawBtn.SetActive(false);
        depositBtn.SetActive(false);

        ammountToAddInput.SetActive(false);
        lastFourDigitInput.SetActive(false);
        BankOptionsGroup.SetActive(false);
        bankImage.SetActive(true);
        ammountCoin.SetActive(true);
        selectedBankAddress.SetActive(false);
        SubmitButton.SetActive(false);
        titleText.text = "Withdraw";

        SaveButton.SetActive(false);
        CloseButton.SetActive(false);
        IFSC.SetActive(false);
        ID.SetActive(false);
        FullName.SetActive(false);
        UPI.SetActive(false);
        PayTM.SetActive(false);
        BankAc.SetActive(false);
        MiniText.SetActive(false);
        NetBalance.SetActive(false);
        Ammount.SetActive(false);
        ReferenceIDInput.SetActive(false);

        AmmountToWithdraw.SetActive(true);
        BankOptionsGroupWithdraw.SetActive(true);
        SubmitWithdraw.SetActive(true);
        MobileNumberWithdraw.SetActive(true);
    }

    public void deposit()
    {
        netBalance.SetActive(false);
        withdrawBtn.SetActive(false);
        depositBtn.SetActive(false);
        BankOptionsGroup.SetActive(true);
        ammountToAddInput.SetActive(true);
        lastFourDigitInput.SetActive(true);
        bankImage.SetActive(true);
        selectedBankAddress.SetActive(true);
        SubmitButton.SetActive(true);
        ammountCoin.SetActive(true);
        ReferenceIDInput.SetActive(true);
        titleText.text = "Deposit";

        SaveButton.SetActive(false);
        CloseButton.SetActive(false);
        IFSC.SetActive(false);
        ID.SetActive(false);
        FullName.SetActive(false);
        UPI.SetActive(false);
        PayTM.SetActive(false);
        BankAc.SetActive(false);
        MiniText.SetActive(false);
        NetBalance.SetActive(false);
        Ammount.SetActive(false);

        AmmountToWithdraw.SetActive(false);
        BankOptionsGroupWithdraw.SetActive(false);
        SubmitWithdraw.SetActive(false);
        MobileNumberWithdraw.SetActive(false);
    }

    public void close()
    {
        netBalance.SetActive(true);
        withdrawBtn.SetActive(true);
        depositBtn.SetActive(true);
        ammountToAddInput.SetActive(false);
        lastFourDigitInput.SetActive(false);
        BankOptionsGroup.SetActive(false);
        bankImage.SetActive(false);
        selectedBankAddress.SetActive(false);
        ammountCoin.SetActive(false);
        SubmitButton.SetActive(false);
        titleText.text = "Withdraw or Deposit";
        WithdrawDetailWindow.SetActive(false);
        ReferenceIDInput.SetActive(false);

        ammountToAddInput.GetComponent<InputField>().text = "";
        lastFourDigitInput.GetComponent<InputField>().text = "";

        AmmountToWithdraw.GetComponent<InputField>().text = "";
        MobileNumberWithdraw.GetComponent<InputField>().text = "";
        ReferenceIDInput.GetComponent<InputField>().text = "";
        

        SaveButton.SetActive(false);
        CloseButton.SetActive(false);
        IFSC.SetActive(false);
        ID.SetActive(false);
        FullName.SetActive(false);
        UPI.SetActive(false);
        PayTM.SetActive(false);
        BankAc.SetActive(false);
        MiniText.SetActive(false);
        NetBalance.SetActive(false);
        Ammount.SetActive(false);

        AmmountToWithdraw.SetActive(false);
        BankOptionsGroupWithdraw.SetActive(false);
        SubmitWithdraw.SetActive(false);
        MobileNumberWithdraw.SetActive(false);
    }

    public void withdrawRequest()
    {
        string url = StaticStrings.baseURL + "api/amount/withdraw";
        int NetAmount = 0;
        bool Done = int.TryParse(FindObjectOfType<InitMenuScript>().DataArray[0], out NetAmount);
        if (!Done)
            NetAmount = 0;

        int amountWithdrawl = 0;
        bool done = int.TryParse(AmmountToWithdraw.GetComponent<InputField>().text, out amountWithdrawl);

        string mobileNumber = MobileNumberWithdraw.GetComponent<InputField>().text;
        if (string.IsNullOrWhiteSpace(mobileNumber) || !done)
        {
            _ShowAndroidToastMessage("Please enter valid amount and mobile number");
            return;
        }

        if (amountWithdrawl <= NetAmount && amountWithdrawl != 0)
        {
            string type = "";
            WWWForm form = new WWWForm();
            form.AddField("playerid", PlayerPrefs.GetString("PID"));
            form.AddField("withdrawmethod", type);
            form.AddField("requestAmount", amountWithdrawl);
            form.AddField("ifsc", "");
            form.AddField("upi_id", "");
            form.AddField("Paytm_ID", "");
            form.AddField("account_number", mobileNumber);
            form.AddField("bank_name", SelectedBankWithdraw);
            UnityWebRequest req = UnityWebRequest.Post(url, form);

            StartCoroutine(withdrawRequestRoutine(req));
        }
        else
        {
            _ShowAndroidToastMessage("Not enough balance to withdraw. You have " + NetAmount + " coins");
        }
    }

    IEnumerator withdrawRequestRoutine(UnityWebRequest request)
    {
        yield return request.SendWebRequest();
        if (request.downloadHandler.text == "success")
        {
            close();
            _ShowAndroidToastMessage("Withdraw request sent successfully");
        }
        else
        {
            close();
            _ShowAndroidToastMessage("Error while processing withdraw request");
        }
    }
}

[Serializable]
public class BankResponse
{
    public string bankNumber;
    public string bank;
}

[Serializable]
public class BankRootResponse
{
    public BankResponse[] banks;
}

public enum RequestType
{
    GET = 0,
    POST = 1,
    PUT = 2
}