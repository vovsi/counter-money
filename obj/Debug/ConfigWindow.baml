   M S B A M L   `   `   `  ����H  DMoreMoreMoney, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:CounterMoneyCounterMoney  5   	       CounterMoney.ConfigWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:CounterMoney     5   	     � Title$  Config��5   	   $	��255��6   $	��300��6%   .��5	      � .+�5
   
   ��  �� Orientation$ Vertical=�6   $��Center=�6,   $	��Top=�6I   .+�5      �� $ 
Horizontal=�6   .+�5      �� .��6   Дата (месяц, год)6+   �� 5      -   5        $��dateTextBox��5      $��130px��6-   $��Right=�6;   {�.  6W   5      �� 5      $ 
Horizontal=�6   .+�5      �� .��6   )'Курс доллара к гривне6/   �� 5      -   5        $��dollarRateUAHTextBox��5      $��100px��66   $��Right=�6D   5      �� 5      $ 
Horizontal=�6   $
��Left=�62   .+�5      �� .��6   -+Реком. пауза в день (мин)63   �� 5      -   5        $!��recommendMaxPauseMinTextBox��5      $
��84px��6=   5      �� 5      $ 
Horizontal=�6   $
��Left=�62   .+�5      �� .��6   0.Заплан. доп. дней (в месяц)65   �� 5      -   5        $��elaborationDaysTextBox��5      $
��75px��68   5      �� 5      $ 
Horizontal=�6   $
��Left=�62   .+�5      �� .��6   ;9Отнять мин. (демо) (за тек. месяц)6<   �� 5      -   5        $��takeMinTextBox��5      $
��40px��60   5      �� 5      $ 
Horizontal=�6   $
��Left=�62   .+�5       �� .��6   75Заработная ставка в долларах66   �� 5!      -   5        $��salaryRateUsdTextBox��5!      $
��60px��66   5"      �� 5#      $ 
Horizontal=�6   $
��Left=�62   .+�5$      �� .��6   #!Норма часов в день6,   �� 5%      -   5        $��hoursRateOfDayTextBox��5%      $��115px��67   5&      �� 5'      -   5        $��saveBtn��5'      $��Сохранить��68   $
��80px��6L   $��0,10,0,0q�6Y   5(      5)      5*      