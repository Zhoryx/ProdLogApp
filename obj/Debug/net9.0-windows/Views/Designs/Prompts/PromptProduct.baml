   M S B A M L   `   `   `  ����E  AProdLogApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullS.clr-namespace:ProdLogApp.Views.Designs.Prompts ProdLogApp.Views.Designs.Prompts  5   	   4    .ProdLogApp.Views.Designs.Prompts.PromptProduct   5   	   W SPresentationCore, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   @local.clr-namespace:ProdLogApp.Views.Designs.Prompts     5   	     � Title$  Seleccionar Producto��5   	   $	��500��5	   	   $	��850��5
   	    � WindowStartupLocation$ CenterScreen=�5   	    � 
ResizeMode$ NoResize=�5   	   ���#0864AC � 	Resources 5       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $ /Views/Designs/Style.xaml?�6%   %    5      .��5      � $��20q�6   .+�5   
   ��  �� Orientation$ Vertical=�6   $
��Left=�6,   $��0,0,0,10q�6G   .+�5      �� $��Buscar por nombre:��6   $��14(�61   
�������$��0,0,0,5q�6R   �� 5      -   5         ��Name$ NombreTextBox��5      $	��250��5      $��40��5         ModernTextBoxStyle#��[  5      $��0,0,0,10q�5      5       �� 5#   
     ShadowBorderStyle#��[ 6    �� Padding$ 10q�5$      $��
280,0,0,80q�5%      $
��Left=�5&      $	��530��5'      ._�5(      �� -   5        	 ��Name$	 ProductList��5(      
 �� View
 5)      �� 5*      .P�5+      ��  �� Width$	 200��6)    �� DisplayMemberBinding 65   �� *Nombre+��5,      �� 5-   "   -   5        $��Nombre��5-   7   
 � Tag$ Nombre��6k   5/      �� 51      $	 200��6)    65   �� *CategoriaNombre+��52      �� 53   "   -   5        $��
Categoría��53   7   $ CategoriaNombre��6n   55      56      58      59      �� 5<   
   $ 
Horizontal=�6   $��Center=�5=      $��Bottom=�5>      $��0,0,0,20q�5?      .+�5@      �� -   5         ��Name$ ConfirmButton��5@      $��	Confirmar��5A        ConfirmButtonStyle#��[ 5B      $	��120��5C      �� 5D      -   5        $ CancelButton��5D      $��Cancelar��5E        CancelButtonStyle#��[ 5F      $	��120��5G      $��10,0,0,0q�5H      5I      5J      5K      