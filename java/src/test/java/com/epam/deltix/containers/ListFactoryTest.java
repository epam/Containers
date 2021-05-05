package com.epam.deltix.containers;

import org.junit.Assert;
import org.junit.Test;

import java.util.ArrayList;
import java.util.Random;

/**
 * Created by DriapkoA on 19/01/2018.
 */
public class ListFactoryTest {

    @Test
    public void stressTest() throws Exception {
        int numberOfIterations = 2000000;
        int numberOfLists = 100;
        ListFactory<Integer> factory = new ListFactory<>();
        ArrayList<Integer> ids = new ArrayList<>();
        ArrayList<BufferedLinkedList<Integer>> etalonLists = new ArrayList<>();
        for (int i = 0; i < numberOfLists; ++i) {
            ids.add(factory.createList());
            etalonLists.add(new BufferedLinkedList<Integer>());
        }
        Random random = new Random(55);
        for (int i = 0; i < numberOfIterations; ++i) {
            int type = Math.abs(random.nextInt()) % 3;
            if (type < 2) {
                int index = Math.abs(random.nextInt()) % numberOfLists;
                int listId = ids.get(index);
                int element = random.nextInt();
                int insertionType = (Math.abs(random.nextInt()) % 4);
                if (insertionType == 0) {
                    factory.addLast(listId, element);
                    etalonLists.get(index).addLast(element);
                } else if (insertionType == 1) {
                    factory.addFirst(listId, element);
                    etalonLists.get(index).addFirst(element);
                } else if (insertionType == 2) {
                    if (etalonLists.get(index).getCount() < 2) continue;
                    int secondId = factory.getNext(factory.getFirst(listId));
                    factory.addAfter(secondId, element);
                    etalonLists.get(index).addAfter(etalonLists.get(index).next(etalonLists.get(index).getFirstKey()), element);
                } else if (insertionType == 3) {
                    if (etalonLists.get(index).getCount() < 2) continue;
                    int secondId = factory.getNext(factory.getFirst(listId));
                    factory.addBefore(secondId, element);
                    etalonLists.get(index).addBefore(etalonLists.get(index).next(etalonLists.get(index).getFirstKey()), element);
                }
            } else {
                int removeType = Math.abs(random.nextInt()) % 3;
                int index = Math.abs(random.nextInt()) % numberOfLists;
                int listId = ids.get(index);
                if (removeType == 0) {
                    if (etalonLists.get(index).getCount() == 0) continue;
                    etalonLists.get(index).remove(etalonLists.get(index).getFirstKey());
                    factory.deleteElement(factory.getFirst(listId));
                } else if (removeType == 1) {
                    if (etalonLists.get(index).getCount() == 0) continue;
                    etalonLists.get(index).remove(etalonLists.get(index).getLastKey());
                    factory.deleteElement(factory.getLast(listId));
                } else if (removeType == 2) {
                    if (etalonLists.get(index).getCount() < 2) continue;
                    etalonLists.get(index).remove(etalonLists.get(index).next(etalonLists.get(index).getFirstKey()));
                    factory.deleteElement(factory.getNext(factory.getFirst(listId)));
                }
            }
            if (i % 10000 == 0) {
                for (int j = 0; j < ids.size(); ++j) {
                    int listId = ids.get(j);
                    Assert.assertEquals(etalonLists.get(j).getCount(), factory.getCount(listId));
                    int ptr1 = etalonLists.get(j).getFirstKey();
                    int ptr2 = factory.getFirst(listId);
                    int n = factory.getCount(listId);
                    for (int k = 0; k < n; ++k) {
                        Assert.assertEquals(etalonLists.get(j).getElementByKey(ptr1), factory.get(ptr2));
                        ptr1 = etalonLists.get(j).next(ptr1);
                        ptr2 = factory.getNext(ptr2);
                    }
                }
            }
        }
        for (int j = 0; j < ids.size(); ++j) {
            int listId = ids.get(j);
            Assert.assertEquals(etalonLists.get(j).getCount(), factory.getCount(listId));
            int ptr1 = etalonLists.get(j).getFirstKey();
            int ptr2 = factory.getFirst(listId);
            int n = factory.getCount(listId);
            for (int k = 0; k < n; ++k) {
                Assert.assertEquals(etalonLists.get(j).getElementByKey(ptr1), factory.get(ptr2));
                ptr1 = etalonLists.get(j).next(ptr1);
                ptr2 = factory.getNext(ptr2);
            }
        }

    }


}
