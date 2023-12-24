using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{

    public GameObject Noitice;
    public GameObject Buy;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Unit a = collision.GetComponent<Unit>();
        if (a != null && a.CompareTag("Player"))
        {
            Debug.Log("Có khách");
            Noitice.gameObject.SetActive(true);
        }
        

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Unit a = collision.GetComponent<Unit>();
        if (a != null && a.CompareTag("Player"))
        {
            Noitice.gameObject.SetActive(false); // ẩn thông báo
        }
    }
}
