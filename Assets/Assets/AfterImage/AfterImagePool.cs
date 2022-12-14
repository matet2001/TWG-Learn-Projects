using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImagePool : MonoBehaviour
{
    public static AfterImagePool Instance;
    
    private Queue<AfterImageController> availableObjects = new Queue<AfterImageController>();
    private AfterImageDataSO afterImageData;
    
    private Vector2 lastImagePosition;
    
    private void Awake()
    {
        Instance = this;  
        afterImageData = Resources.Load<AfterImageDataSO>("AfterImageDataSO");
    }
    private void GrowPool(Vector2 position)
    {
        for (int i = 0; i < 5; i++)
        {
            AfterImageController _newAfterImage = CreateAfterImage(position);
            _newAfterImage.transform.SetParent(transform);
            AddToPool(_newAfterImage); 
        }
    }
    public void AddToPool(AfterImageController _afterImageToAdd)
    {
        _afterImageToAdd.gameObject.SetActive(false);
        availableObjects.Enqueue(_afterImageToAdd);
    }
    private AfterImageController GetFromPool(Vector2 position)
    {
        if (availableObjects.Count == 0) GrowPool(position);

        AfterImageController _currentAfterImage = availableObjects.Dequeue();       

        return _currentAfterImage;
    }
    private AfterImageController CreateAfterImage(Vector2 position)
    {
        AfterImageDataSO _afterImageData = Resources.Load<AfterImageDataSO>("AfterImageDataSO");
        AfterImageController _afterImagePrefab = Resources.Load<AfterImageController>("PfAfterImage");

        AfterImageController _afterImage = Instantiate(_afterImagePrefab, position, Quaternion.identity);
        _afterImage.LoadDefaultDatas(_afterImageData, this);

        return _afterImage;
    }
    public void CreateAfterImageEffect(Transform _targetTransform)
    {
        float distance = Vector2.Distance(_targetTransform.position, lastImagePosition);

        if (distance > afterImageData.distanceBetweenImages)
        {
            AfterImageController _currentAfterImage = GetFromPool(_targetTransform.position);
            _currentAfterImage.RefreshDatas(_targetTransform);
            
            lastImagePosition = _currentAfterImage.transform.position;      
            _currentAfterImage.gameObject.SetActive(true);
        }
    }
}