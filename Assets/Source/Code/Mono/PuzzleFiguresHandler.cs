using System.Collections.Generic;
using Source.Code.Common.Effects;
using Source.Code.Views;
using UnityEngine;

namespace Source.Code.Mono
{
    public class PuzzleFiguresHandler : MonoBehaviour
    {
        #region Serialize Fields
        
        [Header("Puzzle Figures (Skeletons)")]
        [SerializeField] private PuzzleFigureView[] figurePrefabs;
        [SerializeField] private Transform figuresStorage;
        [SerializeField] private GameObject buildingBlock;
        [SerializeField] private PuzzleFigureBlockScript puzzleFigureBlock;

        #endregion

        public PuzzleFigureView[] GetScenePuzzleFigures()
        {
            var skeletons = new PuzzleFigureView[figurePrefabs.Length];
            var index = 0;
            foreach (var figurePrefab in figurePrefabs)
            {
                var figureObject = Instantiate(figurePrefab, figuresStorage, true);
                figureObject.Init();
                figureObject.SetPositionByCenter(new Vector3(index, index, 0));
                ConstructFigure(figureObject, figureObject.transform);
                figureObject.gameObject.SetActive(false);
                skeletons[index] = figureObject;
                index++;
            }

            return skeletons;
        }

        public GameObject GetPuzzleBlock()
        {
            return Instantiate(buildingBlock);
        }

        private void ConstructFigure(PuzzleFigureView view, Transform figure)
        {
            var position = figure.position;
            var blocks = new List<PuzzleFigureBlockScript>();
            
            var b = Instantiate(puzzleFigureBlock, position, Quaternion.identity, figure);
            blocks.Add(b.Init());
            b.gameObject.SetActive(true);
            b.GetComponent<SpriteRenderer>().sortingOrder = 2;
            
            foreach (var blockPosition in view.GetRelativeBlockPositions())
            {
                var block = Instantiate(
                    puzzleFigureBlock, 
                    blockPosition + position, 
                    Quaternion.identity, 
                    figure.transform);
                blocks.Add(block.Init());
                block.gameObject.SetActive(true);
                block.GetComponent<SpriteRenderer>().sortingOrder = 2;
            }
            
            view.InjectBlocks(blocks.ToArray());
        }
    }
}