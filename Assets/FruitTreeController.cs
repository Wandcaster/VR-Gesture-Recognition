using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FruitTreeController : MonoBehaviour
{
    [SerializeField]
    Slider hpSlider;
    [SerializeField]
    Slider hydrationSlider;
    [SerializeField]
    Slider progressSlider;
    [SerializeField]
    float waitTime;
    [SerializeField]
    private float decreaseInHydration;
    [SerializeField]
    private float decreaseInHP;
    [SerializeField]
    private float decreaseHPTriggerValue;
    private List<FruitController> fruits;
    private List<Material> leafMaterials;
    [SerializeField]
    Material LeafMaterial;
    private float defaultSaturationValue;
    private List<FruitSlotController> fruitSlots;
    [SerializeField]
    private float waitTimeForNewFruitCheck;
    [SerializeField]
    private float minHPToProductFruit;
    [SerializeField]
    private GameObject FruitPrefab;
    private void Start()
    {
        fruits= new List<FruitController>(GetComponentsInChildren<FruitController>());
        leafMaterials = GetAllLeafMaterials();
        fruitSlots = new List<FruitSlotController>(GetComponentsInChildren<FruitSlotController>());
        ShuffleList(fruitSlots);
        defaultSaturationValue = leafMaterials[0].GetFloat("Vector1_0a999ce320444fd8ae8d0706b787329e");
        StartCoroutine("UpdateStatus");
        StartCoroutine("UpdateFruitSlots");
    }
    private IEnumerator UpdateStatus()
    {
        while(true)
        {
            yield return new WaitForSeconds(waitTime);
            progressSlider.value = CalculateProgress();
            hydrationSlider.value -= decreaseInHydration;

            if (hydrationSlider.value < decreaseHPTriggerValue) hpSlider.value -= decreaseInHP;
            else hpSlider.value += decreaseInHP;
            ChangeStaturationValue(hpSlider.value * defaultSaturationValue);
        }
    }
    private IEnumerator UpdateFruitSlots()
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTimeForNewFruitCheck);
            if(hpSlider.value > minHPToProductFruit)
            {
                foreach (var fruitSlot in fruitSlots)
                {
                    if (fruitSlot.attachedFruit == null)
                    {
                        fruitSlot.attachedFruit = Instantiate(FruitPrefab, fruitSlot.transform);
                        fruitSlot.attachedFruit.GetComponent<FruitController>().fruitSlot = fruitSlot;
                        break;
                    }
                }
            }
        }
    }
    private float CalculateProgress()
    {
        float growProgress = 0;
        int fruitsToRemove=0;
        foreach (var fruitSlot in fruitSlots)
        {
            if (fruitSlot.attachedFruit!=null) growProgress += fruitSlot.attachedFruit.GetComponent<FruitController>().growProggres;
            else fruitsToRemove++;
        }
        return growProgress / fruitSlots.Count-fruitsToRemove/ fruitSlots.Count;
    }
    private List<Material> GetAllLeafMaterials()
    {
        List<Material> leafMaterials = new List<Material>();
        foreach (var meshRenderer in GetComponentsInChildren<MeshRenderer>())
        {
            for (int i=0;i<meshRenderer.sharedMaterials.Length;i++)
            {
                if (meshRenderer.sharedMaterials[i] == LeafMaterial) leafMaterials.Add(meshRenderer.materials[i]);
            }
        }
        return leafMaterials;
    }
    private void ChangeStaturationValue(float value)
    {
        foreach (var material in leafMaterials)
        {
            material.SetFloat("Vector1_0a999ce320444fd8ae8d0706b787329e",value);
        }
    }
    private void ShuffleList<T>(IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0,n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
    public void WaterTree(float amount)
    {
        Debug.Log("Try to water tree");
        hydrationSlider.value += amount;
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<WaterBall>()) WaterTree(collision.gameObject.GetComponent<WaterBall>().hydrationValue);
    }

}
