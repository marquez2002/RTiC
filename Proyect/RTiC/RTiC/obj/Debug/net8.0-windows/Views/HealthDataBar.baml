   M S B A M L   `   `   `  ����?  ;RTIC, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:RTIC.ViewModelsRTIC.ViewModels  5          RTIC.Views.HealthDataBar   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089K GUno.Xaml, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null<x,http://schemas.microsoft.com/winfx/2006/xaml      5      .vmclr-namespace:RTIC.ViewModels      5      $
��Auto��5      $	��250��6   .��5      � ���#2D2D30.+�5   
   �� $��10q�6   .+�5
      �� $��Perfil del Corredor��6   $��20(�63   
�������$
��Bold"�6T   $��0 0 0 20q�6f   �� 5      $��Nombre:��6   
�������$��15(�6:   $
��Bold"�6H   �� 5      $��15(�65   $
��Bold"�6C   
�������$��0 0 0 10q�6h   $
��Wrap=�6z   $��Segoe UI Semibold)�6�   ��6   �� *Runner.Name+6�   �� 5      $��Edad:��6   
�������$��15(�68   $
��Bold"�6F   �� 5      $��0 0 0 10q�6W   $��15(�6i   ��6   �� *
Runner.Age+  �� UpdateSourceTrigger$  PropertyChanged=�6i   �� 5      $��Altura (cm):��6   
�������$��15(�6?   $
��Bold"�6M   �� 5      $��0 0 0 10q�6Z   $��15(�6l   ��6   �� *Runner.Height+$  PropertyChanged=�6l   �� 5      $��
Peso (kg):��6   
�������$��15(�6=   $
��Bold"�6K   �� 5      $��0 0 0 10q�6Z   $��15(�6l   ��6   �� *Runner.Weight+$  PropertyChanged=�6l   �� 5      $��Nivel:��6   
�������$��15(�69   $
��Bold"�6G   �� 5      $��0 0 0 10q�5      $��15(�5      ��5      �� *	Niveles+ �� SelectedItem 5      �� *Runner.Level+ �� Mode$ TwoWay=�$  PropertyChanged=�5      �� 5      $��Guardar Perfil��6   $��0 10 0 0q�6U   $��15(�6g   ��6.   �� *SaveProfileCommand+6g   �� 5"      $��0 10 0 0q�6   .+�5#      �� $	��IMC��6   
�������$��0 0 0 5q�6:   $��15(�6K   $
��Bold"�6Y   � 5$      $��30��6;    � Minimum$ 0Q�6G    � Maximum$	 100Q�6S    � Value 6   �� *
Runner.Imc+6S   5%      �� 5(      $��0 10 0 0q�6A   ��6   �� *WeightHistory+��5)      �� 5*      .R�5+      �� 
�������$��14(�6H   $	��0 2q�6V   ��6$   �� 6V   5,      5.      �� 51      $��Cordoba, Spain��6   $��16(�6.   
�������$��0 10 0 0q�6O   �� 52      $��16(�69   
���������6   �� *TemperatureText+6G   �� 53      $��16(�62   
���������6   �� *
WindText+6@   �� 55      $�� ��6   $��16(�6!   
�������$��0 10 0 0q�6B   �� 58      $��14(�64   
�������$
��Wrap=�6U   $��0 10 0 0q�6i   ��6   �� *
AdviceText+6i   59      5:      5;      