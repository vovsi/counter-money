   M S B A M L   `   `   `  ����O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem  5   	   H DMoreMoreMoney, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:CounterMoneyCounterMoney 5   	      CounterMoney.VerifyWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   6sys&clr-namespace:System;assembly=mscorlib     5   	   ,localclr-namespace:CounterMoney     5   	     � Title$  VerifyWindow��5	   	   $	��300��6   $	��520��6+   .��5
      ��  �� Orientation$ Vertical=�6   .+�5   
   �� -   5        $��daysListView��5      $	��240��6'    �� View 5      �� 5      .P�5      �� $��Дата��6%    �� Width$ 80��63    �� DisplayMemberBinding 6>   ��  �� Path$
 Date� �� StringFormat$ 
dd.MM.yyyy���� 5      $��Отработано��6%   $ 80��69    6D   �� *TimeWorkText+�� 5      $#��Отработано (сек)��6%   $	 100��6?    6K   �� *SecondsWork+�� 5      $��Перерыв (сек)��6%   $ 90��6<    6G   �� *SecondsPause+�� 5      $��Тип��6%   $ 70��62    6=   �� *Type+�� 5      $��
Вериф��6%   $ 50��64    6?   �� *Verify+5      5      �� 5   
   $ 
Horizontal=�6   $��Center=�6.   $��Bottom=�6K   .+�5      �� .��6   Всего:6   �� 5      -   5        $��allTime��5      6%   5      5      5      