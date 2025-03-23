namespace YesLocation.Domain.Enums;

public enum VehiclePartLocation
{
  // Parties communes à plusieurs vues
  Hood,              // Capot
  Roof,              // Toit
  Trunk,             // Coffre

  // Parties avant
  FrontBumper,       // Pare-chocs avant
  Headlights,        // Phares
  Grille,            // Calandre

  // Parties latérales
  FrontLeftDoor,     // Portière avant gauche
  RearLeftDoor,      // Portière arrière gauche
  FrontRightDoor,    // Portière avant droite
  RearRightDoor,     // Portière arrière droite
  LeftMirror,        // Rétroviseur gauche
  RightMirror,       // Rétroviseur droit
  LeftFender,        // Aile gauche
  RightFender,       // Aile droite

  // Parties arrière
  RearBumper,        // Pare-chocs arrière
  Taillights,        // Feux arrière

  // Roues
  FrontLeftWheel,    // Roue avant gauche
  RearLeftWheel,     // Roue arrière gauche
  FrontRightWheel,   // Roue avant droite
  RearRightWheel,    // Roue arrière droite

  // Vitres
  Windshield,        // Pare-brise
  RearWindow,        // Lunette arrière
  LeftWindows,       // Vitres côté gauche
  RightWindows,      // Vitres côté droit

  // Parties intérieures
  Dashboard,         // Tableau de bord
  SteeringWheel,     // Volant
  GearShift,         // Levier de vitesse
  FrontSeats,        // Sièges avant
  RearSeats,         // Sièges arrière
  FloorMats,         // Tapis de sol
  InfainmentSystem,  // Système d'info-divertissement
  AirConditioning,   // Climatisation/chauffage
  InteriorLights,    // Éclairage intérieur
  GloveBox,          // Boîte à gants
  TrunkInterior,     // Intérieur du coffre

  // Autre
  Other              // Autre partie (à préciser dans la description)
}