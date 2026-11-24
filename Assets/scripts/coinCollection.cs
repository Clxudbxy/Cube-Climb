using UnityEngine;

public class coinCollection : MonoBehaviour
{
    private bool _collect;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _collect = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(_collect)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        if(c.gameObject.tag == "Player")
        {
            _collect = true;
        }
    }

    private void OnTriggerExit2D(Collider2D c)
    {
        if(c.gameObject.tag == "Player")
        {
            _collect = false;
        }
    }
}
