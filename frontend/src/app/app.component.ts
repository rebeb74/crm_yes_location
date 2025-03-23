import { CommonModule } from '@angular/common';
import { Component, HostBinding, OnDestroy, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { Subject } from 'rxjs';
import { ThemeService } from './core/services/theme.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, FormsModule, NgbModule],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit, OnDestroy {
  title = 'YesLocation CRM';
  favoriteFruit: string = 'Pomme';
  inputValue: string = '';

  @HostBinding('class.app-light-theme') lightTheme = true;
  @HostBinding('class.app-dark-theme') darkTheme = false;

  private destroy$ = new Subject<void>();

  constructor(private themeService: ThemeService) {}

  ngOnInit() {
    this.themeService.loadTheme();

    this.themeService.isDarkMode$.subscribe((isDark) => {
      this.darkTheme = isDark;
      this.lightTheme = !isDark;
      document.documentElement.setAttribute(
        'data-bs-theme',
        isDark ? 'dark' : 'light'
      );
    });
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }

  toggleDarkMode() {
    this.themeService.toggleDarkMode();
  }

  get isDarkMode() {
    return this.themeService.isDarkMode;
  }
}
