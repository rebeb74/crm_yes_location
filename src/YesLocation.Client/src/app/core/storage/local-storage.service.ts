import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class LocalStorageService {
  constructor() {}

  /**
   * Enregistre une valeur dans le localStorage
   */
  set(key: string, value: any): void {
    try {
      localStorage.setItem(key, JSON.stringify(value));
    } catch (error) {
      console.error("Erreur lors de l'enregistrement dans localStorage", error);
    }
  }

  /**
   * Récupère une valeur du localStorage
   */
  get<T>(key: string): T | null {
    try {
      const item = localStorage.getItem(key);
      return item ? JSON.parse(item) : null;
    } catch (error) {
      console.error(
        'Erreur lors de la récupération depuis localStorage',
        error
      );
      return null;
    }
  }

  /**
   * Supprime une valeur du localStorage
   */
  remove(key: string): void {
    try {
      localStorage.removeItem(key);
    } catch (error) {
      console.error('Erreur lors de la suppression depuis localStorage', error);
    }
  }

  /**
   * Vide entièrement le localStorage
   */
  clear(): void {
    try {
      localStorage.clear();
    } catch (error) {
      console.error('Erreur lors du vidage du localStorage', error);
    }
  }
}
