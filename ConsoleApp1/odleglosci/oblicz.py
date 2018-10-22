#!/usr/bin/python
#-*- coding: utf-8 -*-
import sys
import string
from math import sqrt
 # Wywołania: " python zmieniacz.py nazwa_pliku_we" (np.: "python zmieniacz.py berlin52.txt wyniki.txt")
def zbudujMacierz(wiersze):
    macierz = [[0 for col in range(int(wiersze[0]))] for row in range(int(wiersze[0]))]
    i = 0
    for w in wiersze[1:]:
        w = string.strip(w).split(' ')
        j = 0
        for odl in w:
            macierz[i][j] = int(odl)
            macierz[j][i] = int(odl)
            j+=1
        i+=1

    return macierz

def obliczOdleglosc(trasa, macierz):
    odleglosc = 0
    trasa = string.strip(trasa).split('-')
    try:
        trasa = list(map(int, trasa))
    except ValueError:
        pass
    for i in range(len(trasa)-1):
        odleglosc+=macierz[trasa[i]][trasa[i+1]]
    odleglosc+=macierz[trasa[len(trasa)-1]][trasa[0]]
    return odleglosc

nPliku = sys.argv[1]
nPlikuWynikow = sys.argv[2]

plikWe = open(nPliku, 'r') # Otwarcie pliku wejściowego
plikWe2 = open(nPlikuWynikow, 'r') # Otwarcie pliku wejściowego wyników

plikWy = open('spr_'+nPliku.split('.')[0]+'.txt', 'w') # Stworzenie nowego pliku (wyjściowego) o tej samej nazwie, co wejściowy, ale z rozszerzeniem .txt

wiersze = plikWe.readlines() # linie pliku do listy
plikWe.close()

macierz = zbudujMacierz(wiersze)

wyniki = plikWe2.readlines() # linie pliku do listy
plikWe2.close()

for w in wyniki:
    odleglosc = 0
    w = string.strip(w).split(' ') # Podział linii według spacji, a przed tym wycięcie zbędnych białych znaków
    if len(w)==2:
        odleglosc = obliczOdleglosc(w[0], macierz)
        wynik = "%i %i %s" % (odleglosc, int(w[1]), odleglosc==int(w[1]))
    else:
        wynik = "Pominięto: %s" % w
    print wynik
    plikWy.write(wynik+'\n') # Zapisanie do pliku
       
plikWy.close()

