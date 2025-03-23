import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { AgencyDto } from '../api/kiota-generated/models';
import { AgencyService } from './agency.service';

interface AgencyTheme {
  primaryColor: string;
  secondaryColor: string;
  accentColor: string;
  successColor: string;
  warningColor: string;
  errorColor: string;
  useDarkMode: boolean;
  darkModePrimaryColor: string;
  darkModeSecondaryColor: string;
  darkModeAccentColor: string;
  darkModeSuccessColor: string;
  darkModeWarningColor: string;
  darkModeErrorColor: string;
}

@Injectable({
  providedIn: 'root',
})
export class ThemeService {
  private isDarkModeSubject = new BehaviorSubject<boolean>(false);
  isDarkMode$ = this.isDarkModeSubject.asObservable();

  private defaultTheme: AgencyTheme = {
    primaryColor: '#0061CE',
    secondaryColor: '#40B187',
    accentColor: '#8531A9',
    successColor: '#28a745',
    warningColor: '#ffc107',
    errorColor: '#dc3545',
    useDarkMode: false,
    darkModePrimaryColor: '#005bc1',
    darkModeSecondaryColor: '#338e6c',
    darkModeAccentColor: '#6a2787',
    darkModeSuccessColor: '#218838',
    darkModeWarningColor: '#e0a800',
    darkModeErrorColor: '#c82333',
  };

  private currentTheme: AgencyTheme = this.defaultTheme;

  constructor(private agencyService: AgencyService) {
    // Vérifier la préférence système au démarrage
    this.checkSystemPreference();

    // Charger le thème de l'agence
    this.loadAgencyTheme();
  }

  private checkSystemPreference() {
    const prefersDark = window.matchMedia(
      '(prefers-color-scheme: dark)'
    ).matches;
    this.isDarkModeSubject.next(prefersDark);

    // Écouter les changements de préférence système
    window
      .matchMedia('(prefers-color-scheme: dark)')
      .addEventListener('change', (e) => {
        this.isDarkModeSubject.next(e.matches);
        this.applyTheme(e.matches);
      });
  }

  loadTheme() {
    // Charger le thème initial
    const isDark = this.isDarkModeSubject.value;
    this.applyTheme(isDark);
  }

  toggleDarkMode() {
    const currentValue = this.isDarkModeSubject.value;
    this.isDarkModeSubject.next(!currentValue);
    this.applyTheme(!currentValue);
  }

  private loadAgencyTheme() {
    console.log("Chargement du thème de l'agence...");
    this.agencyService.loadAgency().subscribe({
      next: (agency: AgencyDto | null) => {
        if (agency) {
          console.log('Agence trouvée:', agency);
          // Mettre à jour le thème avec les valeurs de l'agence
          this.currentTheme = {
            primaryColor: agency.primaryColor || this.defaultTheme.primaryColor,
            secondaryColor:
              agency.secondaryColor || this.defaultTheme.secondaryColor,
            accentColor: agency.accentColor || this.defaultTheme.accentColor,
            successColor: agency.successColor || this.defaultTheme.successColor,
            warningColor: agency.warningColor || this.defaultTheme.warningColor,
            errorColor: agency.errorColor || this.defaultTheme.errorColor,
            useDarkMode: agency.useDarkMode ?? this.defaultTheme.useDarkMode,
            darkModePrimaryColor:
              agency.darkModePrimaryColor ||
              this.defaultTheme.darkModePrimaryColor,
            darkModeSecondaryColor:
              agency.darkModeSecondaryColor ||
              this.defaultTheme.darkModeSecondaryColor,
            darkModeAccentColor:
              agency.darkModeAccentColor ||
              this.defaultTheme.darkModeAccentColor,
            darkModeSuccessColor:
              agency.darkModeSuccessColor ||
              this.defaultTheme.darkModeSuccessColor,
            darkModeWarningColor:
              agency.darkModeWarningColor ||
              this.defaultTheme.darkModeWarningColor,
            darkModeErrorColor:
              agency.darkModeErrorColor || this.defaultTheme.darkModeErrorColor,
          };
          console.log('Thème mis à jour:', this.currentTheme);

          // Appliquer le thème
          this.applyTheme(this.currentTheme.useDarkMode);
        } else {
          console.log(
            'Aucune agence trouvée, utilisation du thème par défaut:',
            this.defaultTheme
          );
          this.currentTheme = this.defaultTheme;
          this.applyTheme(this.defaultTheme.useDarkMode);
        }
      },
      error: (error: unknown) => {
        console.error('Erreur lors du chargement du thème :', error);
        this.currentTheme = this.defaultTheme;
        this.applyTheme(this.defaultTheme.useDarkMode);
      },
    });
  }

  private applyTheme(isDark: boolean) {
    console.log('Application du thème:', {
      isDark,
      currentTheme: this.currentTheme,
    });

    // Appliquer les variables CSS personnalisées
    const root = document.documentElement;

    // Appliquer les couleurs de base
    if (isDark) {
      console.log('Application du thème sombre');
      root.style.setProperty('--bs-body-bg', '#1a1a1a');
      root.style.setProperty('--bs-body-color', '#e9ecef');
      root.style.setProperty('--bs-surface', '#2d2d2d');

      // Couleurs en mode sombre
      root.style.setProperty(
        '--bs-primary',
        this.currentTheme.darkModePrimaryColor
      );
      root.style.setProperty(
        '--bs-primary-rgb',
        this.hexToRgb(this.currentTheme.darkModePrimaryColor)
      );
      root.style.setProperty(
        '--bs-secondary',
        this.currentTheme.darkModeSecondaryColor
      );
      root.style.setProperty(
        '--bs-secondary-rgb',
        this.hexToRgb(this.currentTheme.darkModeSecondaryColor)
      );
      root.style.setProperty(
        '--bs-accent',
        this.currentTheme.darkModeAccentColor
      );
      root.style.setProperty(
        '--bs-accent-rgb',
        this.hexToRgb(this.currentTheme.darkModeAccentColor)
      );
      root.style.setProperty(
        '--bs-success',
        this.currentTheme.darkModeSuccessColor
      );
      root.style.setProperty(
        '--bs-success-rgb',
        this.hexToRgb(this.currentTheme.darkModeSuccessColor)
      );
      root.style.setProperty(
        '--bs-warning',
        this.currentTheme.darkModeWarningColor
      );
      root.style.setProperty(
        '--bs-warning-rgb',
        this.hexToRgb(this.currentTheme.darkModeWarningColor)
      );
      root.style.setProperty(
        '--bs-danger',
        this.currentTheme.darkModeErrorColor
      );
      root.style.setProperty(
        '--bs-danger-rgb',
        this.hexToRgb(this.currentTheme.darkModeErrorColor)
      );
    } else {
      console.log('Application du thème clair');
      root.style.setProperty('--bs-body-bg', '#ffffff');
      root.style.setProperty('--bs-body-color', '#212529');
      root.style.setProperty('--bs-surface', '#f8f9fa');

      // Couleurs en mode clair
      root.style.setProperty('--bs-primary', this.currentTheme.primaryColor);
      root.style.setProperty(
        '--bs-primary-rgb',
        this.hexToRgb(this.currentTheme.primaryColor)
      );
      root.style.setProperty(
        '--bs-secondary',
        this.currentTheme.secondaryColor
      );
      root.style.setProperty(
        '--bs-secondary-rgb',
        this.hexToRgb(this.currentTheme.secondaryColor)
      );
      root.style.setProperty('--bs-accent', this.currentTheme.accentColor);
      root.style.setProperty(
        '--bs-accent-rgb',
        this.hexToRgb(this.currentTheme.accentColor)
      );
      root.style.setProperty('--bs-success', this.currentTheme.successColor);
      root.style.setProperty(
        '--bs-success-rgb',
        this.hexToRgb(this.currentTheme.successColor)
      );
      root.style.setProperty('--bs-warning', this.currentTheme.warningColor);
      root.style.setProperty(
        '--bs-warning-rgb',
        this.hexToRgb(this.currentTheme.warningColor)
      );
      root.style.setProperty('--bs-danger', this.currentTheme.errorColor);
      root.style.setProperty(
        '--bs-danger-rgb',
        this.hexToRgb(this.currentTheme.errorColor)
      );
    }

    // Vérifier les valeurs appliquées
    console.log('Variables CSS appliquées:', {
      primary: root.style.getPropertyValue('--bs-primary'),
      primaryRgb: root.style.getPropertyValue('--bs-primary-rgb'),
      secondary: root.style.getPropertyValue('--bs-secondary'),
      secondaryRgb: root.style.getPropertyValue('--bs-secondary-rgb'),
      accent: root.style.getPropertyValue('--bs-accent'),
      accentRgb: root.style.getPropertyValue('--bs-accent-rgb'),
      success: root.style.getPropertyValue('--bs-success'),
      successRgb: root.style.getPropertyValue('--bs-success-rgb'),
      warning: root.style.getPropertyValue('--bs-warning'),
      warningRgb: root.style.getPropertyValue('--bs-warning-rgb'),
      danger: root.style.getPropertyValue('--bs-danger'),
      dangerRgb: root.style.getPropertyValue('--bs-danger-rgb'),
      bodyBg: root.style.getPropertyValue('--bs-body-bg'),
      bodyColor: root.style.getPropertyValue('--bs-body-color'),
      surface: root.style.getPropertyValue('--bs-surface'),
    });

    // Appliquer le thème Bootstrap
    document.documentElement.setAttribute(
      'data-bs-theme',
      isDark ? 'dark' : 'light'
    );
    console.log(
      'Attribut data-bs-theme:',
      document.documentElement.getAttribute('data-bs-theme')
    );
  }

  private hexToRgb(hex: string): string {
    const result = /^#?([a-f\d]{2})([a-f\d]{2})([a-f\d]{2})$/i.exec(hex);
    if (result) {
      const r = parseInt(result[1], 16);
      const g = parseInt(result[2], 16);
      const b = parseInt(result[3], 16);
      return `${r}, ${g}, ${b}`;
    }
    return '0, 0, 0';
  }

  get isDarkMode(): boolean {
    return this.isDarkModeSubject.value;
  }
}
