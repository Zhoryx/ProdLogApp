   M S B A M L   `   `   `  ����E  AProdLogApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullS.clr-namespace:ProdLogApp.Views.Designs.Prompts ProdLogApp.Views.Designs.Prompts  5   	   5    /ProdLogApp.Views.Designs.Prompts.PromptCategory   5   	   W SPresentationCore, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression.blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   @local.clr-namespace:ProdLogApp.Views.Designs.Prompts     5   	     � Title$  Seleccionar Categoría��5   	   $	��500��6'   $	��850��64    � WindowStartupLocation$ CenterScreen=�5	   	    � 
ResizeMode$ NoResize=�5
   	   ���#0864AC � 	Resources 5       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $ /Views/Designs/Style.xaml?�6%   %    5      .��5      � $��20q�6   .+�5   
   ��  �� Orientation$ Vertical=�6   $
��Left=�6,   $��10,10,0,322q�6G   .+�5      �� $��Buscar por nombre:��6   $��14(�61   
�������$��0,0,0,5q�6R   �� 5      -   5         ��Name$ 	SearchBox��5      $	��250��6)   $��40��65      ModernTextBoxStyle#��[  6A   5      5      �� 5   
   $ Vertical=�6   $
��Left=�6,   $��10,127,0,222q�6G   .+�5      �� $��Buscar por Código:��6   $��14(�61   
�������$��0,0,0,5q�6R   �� 5      -   5        $ SearchBoxID��5      $	��250��6+   $��40��67   #��[  6C   5      5       �� 5'   
     ShadowBorderStyle#��[ 6    �� Padding$ 10q�6<   $��
303,0,0,70q�6I   $
��Left=�6]   $	��530��6x   ._�5(      �� -   5        	 ��Name$	 CategoryList��5(      $	��320��6-   
 �� SelectionMode$
 Single=�6:   $��
0,17,21,17q�6�    �� View 5)      �� 5*      .P�5+      ��  �� Width$ 80��6)    �� DisplayMemberBinding 64   �� *
CategoryId+��5,      �� 5-   "   -   5        $��Código��5-   7   
 � Tag$ 
CategoryId��6k   5/      �� 51      $	 400��6)    65   �� *Nombre+��52      �� 53   "   -   5        $��Nombre de Categoría��53   7   $ Nombre��6x   55      56      58      5:      �� 5=   
   $ 
Horizontal=�6   $
��Left=�6.   $��Bottom=�6I   $��	10,0,0,10q�6d   .+�5>      �� -   5         ��Name$ ConfirmButton��5>      $��	Confirmar��6,     ConfirmButtonStyle#��[ 6@   $	��150��6l   $��10q�6x   6�   �� 5?      -   5        $ CancelButton��5?      $��Cancelar��6+     CancelButtonStyle#��[ 6>   $	��150��6i   5@      5A      5B      