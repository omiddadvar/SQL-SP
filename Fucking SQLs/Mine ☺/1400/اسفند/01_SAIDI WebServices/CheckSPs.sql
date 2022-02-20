
---Step 1
EXEC spSimaBase '' ,'1400/11/01' ,'1400/11/31'

---Step 2
EXEC spSimaSaidi  '1400/11/01' ,'1400/11/31' ,''

---Step 3
EXEC spSimaBohrani '1400/11/01' ,'1400/11/31' , ''

---Step 4
EXEC spSimaDisFeeder '1400/11/01' ,'1400/11/31' , ''

---Step 5
EXEC spSimaService '1400/11/01' ,'1400/11/31' , ''

---Step 6
EXEC spSimaModat '1400/11/01' ,'1400/11/31' , ''

---Step 7
EXEC spSimaEnergy '1400/11/01' ,'1400/11/31' , ''

