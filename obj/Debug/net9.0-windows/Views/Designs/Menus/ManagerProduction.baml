   M S B A M L   `   `   `  ����E  AProdLogApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:ProdLogApp.ViewsProdLogApp.Views  5   	   X TXceed.Wpf.Toolkit, Version=4.7.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f4P:clr-namespace:Xceed.Wpf.Toolkit;assembly=Xceed.Wpf.ToolkitXceed.Wpf.Toolkit 5   	   (    "ProdLogApp.Views.ManagerProduction   5   	   W SPresentationCore, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:ProdLogApp.Views     5   	   Kxctk:clr-namespace:Xceed.Wpf.Toolkit;assembly=Xceed.Wpf.Toolkit     5   	     � Title$  Menu - Producciones��5	   	   $	��720��5
   	   $
��1151��5   	    � WindowStartupLocation$ CenterScreen=�5   	   ���#0864AC � 
ResizeMode$ NoResize=�5   	    � 	Resources 5       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $ /Views/Designs/Style.xaml?�6%   %    5      .��5      � $��0,0,10,0q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6    � ColumnDefinitions 5!   
   �� 5"      $��3* �6   �� 5#      $��2* �6   .+�5&   
   �� $��0��6   ���#E6F0FA   ShadowBorderStyle#��[  63   $��110,10,110,10q�6^    �� Padding$ 10q�6u   $��2��6�   ._�5'      � $��Center=�6   $��Center=�60   .+�5(      �� 	 �� Orientation$	 
Horizontal=�6   $��Center=�66   $��Center=�6S   .+�5+      �� $��10q�6!   .+�5,      �� $��Buscar Operario��6    $��14(�6:   ���#0864AC$��Center=�6]   )  #Xceed.Wpf.Toolkit.AutoSelectTextBox  5-      -   5        
  Name$
 OperarioAutoComplete��5-   1   $	��200��5.   (   $��38��64   $��Center=�6@   5/      �� 52      $��10q�6!   .+�53      �� $��Fecha Desde��6    $��14(�66   ���#0864AC$��Center=�6Y   (  "System.Windows.Controls.DatePicker  54      -   5          Name$ FechaDesdePicker��54   %   $	��200��6?   $��30��6K   $��Center=�6W   55      �� 58      $��10q�6!   .+�59      �� $��Fecha Hasta��6    $��14(�66   ���#0864AC$��Center=�6Y     5:      -   5        $ FechaHastaPicker��5:   %   $	��200��6?   $��30��6K   $��Center=�6W   5;      �� 5>      $��10q�6!   .+�5?      �� $��Buscar Producto��6    $��14(�6:   ���#0864AC$��Center=�6]     5@      -   5        $
 ProductoAutoComplete��5@   1   $	��200��5A   (   $��38��64   $��Center=�6@   5B      5C      5D      5E      �� 5J   
   $��1��6   ���#E6F0FA#��[  63   $��110,10,110,10q�6^   $ 10q�6u   $��2��6�    J� RenderTransformOrigin$ 0.503,0.528&�6�   ._�5K      �� $
��Auto=�6   .��5L      �� -   5         ��Name$ Producciones��5L      $	��884��61   $ 0.664,0.535&�6=    �� View 5M      �� 5N      .P�5O      ��  �� Width$
 Auto��6-   $��Operario��6:    �� DisplayMemberBinding 6L   �� *
Operario+�� 5P      $
 Auto��6-   $��Producto��6:    6L   �� *
Producto+�� 5Q      $
 Auto��6-   $��Cantidad��6:    6L   �� *
Cantidad+�� 5R      $
 Auto��6-   $��Hora Inicio��6:    6O   �� *	HInicio+ �� StringFormat$ HH:mm���� 5S      $
 Auto��6-   $��Hora Fin��6:    6L   �� *HFin+$ HH:mm��5T      5V      5W      5X      �� 5[   
   $��2��6   $	 
Horizontal=�6"   $
��Left=�6;   $��236,10,0,10q�6V   $	��662��6k   $��2��6w   .+�5\      ��   ModernButtonStyle#��[ 6   $��Agregar��6@   $��10q�6R   �� 5]      #��[ 6   $��Eliminar��6@   $��10q�6S   �� 5^      #��[ 6   $��	Modificar��6@   $��10q�6T   5_      �� 5b   
   $��2��6   $	 
Horizontal=�6"   $ 
0.51,0.517&�6;   $��251,78,-13,7q�6^   $��2��6t   $��1��6�   .+�5c      �� -   5        #��[ 5c      $��Volver��6@   6Q   5d      5e      5f      