using System;
using System.Collections.Generic;
using UnityEngine;

namespace GonadFateSim.UI
{
    public sealed class ModalManager : MonoBehaviour
    {
        private readonly List<ModalEntry> openModals = new List<ModalEntry>();

        public static ModalManager Instance { get; private set; }

        public bool HasOpenModal => FindTopmostIndex() >= 0;

        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        public void RegisterOpen(GameObject modalRoot, Action closeAction)
        {
            if (modalRoot == null)
            {
                return;
            }

            Unregister(modalRoot);
            openModals.Add(new ModalEntry(modalRoot, closeAction));
            modalRoot.transform.SetAsLastSibling();
        }

        public void Unregister(GameObject modalRoot)
        {
            if (modalRoot == null)
            {
                return;
            }

            for (int i = openModals.Count - 1; i >= 0; i--)
            {
                if (openModals[i].Root == null || openModals[i].Root == modalRoot)
                {
                    openModals.RemoveAt(i);
                }
            }
        }

        public bool CloseTopmost()
        {
            int index = FindTopmostIndex();
            if (index < 0)
            {
                return false;
            }

            ModalEntry entry = openModals[index];
            openModals.RemoveAt(index);
            if (entry.CloseAction != null)
            {
                entry.CloseAction.Invoke();
            }
            else if (entry.Root != null)
            {
                entry.Root.SetActive(false);
            }

            return true;
        }

        private int FindTopmostIndex()
        {
            for (int i = openModals.Count - 1; i >= 0; i--)
            {
                GameObject root = openModals[i].Root;
                if (root == null)
                {
                    openModals.RemoveAt(i);
                    continue;
                }

                if (root.activeInHierarchy)
                {
                    return i;
                }

                openModals.RemoveAt(i);
            }

            return -1;
        }

        private readonly struct ModalEntry
        {
            public readonly GameObject Root;
            public readonly Action CloseAction;

            public ModalEntry(GameObject root, Action closeAction)
            {
                Root = root;
                CloseAction = closeAction;
            }
        }
    }
}
