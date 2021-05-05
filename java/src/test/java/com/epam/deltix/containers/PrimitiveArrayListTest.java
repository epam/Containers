package com.epam.deltix.containers;

import com.epam.deltix.containers.generated.IntArrayList;
import com.epam.deltix.containers.generated.LongArrayList;
import com.epam.deltix.dfp.Decimal64;
import org.junit.Assert;
import org.junit.Test;

import java.util.ArrayList;
import java.util.Collections;
import java.util.Iterator;
import java.util.Random;

/**
 * Created by DriapkoA on 31/01/2018.
 */
public class PrimitiveArrayListTest {

    @Test
    public void stressTest()
    {
        IntArrayList list = new IntArrayList();
        ArrayList etalonList = new ArrayList();
        Assert.assertTrue(list.isEmpty());
        Random r = new Random();
        int num = 1000;
        for(int i = 1; i <= num; i++)
        {
            int x = r.nextInt();
            list.add(x);
            etalonList.add(x);
            Assert.assertEquals(list.contains(x), etalonList.contains(x));

            int y = r.nextInt() % list.size();
            if(y < 0) y *= -1;
            list.add(y, x / 2);
            etalonList.add(y, x / 2);
            Assert.assertEquals(list.indexOf(x / 2), etalonList.indexOf(x / 2));
            Assert.assertEquals(list.lastIndexOf(x / 2), etalonList.lastIndexOf(x / 2));
        }
        Assert.assertFalse(list.isEmpty());

        for(int i = 1; i <= num / 10; i++)
        {
            int x = r.nextInt() % list.size();
            if(x < 0) x *= -1;
            list.removeByIndex(x);
            etalonList.remove(x);
            Assert.assertEquals(list.contains(x), etalonList.contains(x));

            x = r.nextInt() % etalonList.size();
            if(x < 0) x *= -1;
            list.remove((int)etalonList.get(x));
            etalonList.remove(etalonList.get(x));
            Assert.assertEquals(list.contains(x), etalonList.contains(x));
        }
        for(int i = 1; i <= num; i++)
        {
            int x = r.nextInt() % list.size();
            if(x < 0) x *= -1;
            Assert.assertEquals(list.get(x), etalonList.get(x));
        }
        IntArrayList list2 = (IntArrayList)list.clone();
        Assert.assertTrue(list.equals(list2));
        Assert.assertTrue(list2.equals(list));
        list2.removeByIndex(0);
        Assert.assertFalse(list.equals(list2));
        Assert.assertFalse(list2.equals(list));

        list.clear();
        Assert.assertTrue(list.isEmpty());
    }

    @Test
    public void resizeTest() {
        LongArrayList list = new LongArrayList(2);
        Random r = new Random();
        int num = 1024;
        for(int i = 1; i <= num; i++)
        {
            int x = r.nextInt();
            list.add(x);
        }
        Assert.assertEquals(list.getCapacity(), 1064);
        Assert.assertEquals(list.getGrowFactor(), 1.5, 0.001);
        for (int i = 0; i < 24; i++) {
            list.removeByIndex(i);
        }
        Assert.assertEquals(list.getCapacity(), 1064);
        list.shrinkToFit();
        Assert.assertEquals(list.getCapacity(), 1000);
        list.setGrowFactor(20);
        for(int i = 1; i <= num / 10; i++)
        {
            int x = r.nextInt();
            list.add(x);
        }
        Assert.assertEquals(list.getCapacity(), 20001);
    }


}
