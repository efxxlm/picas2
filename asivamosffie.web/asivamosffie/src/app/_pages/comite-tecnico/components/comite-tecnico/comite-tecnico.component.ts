import { Component, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-comite-tecnico',
  templateUrl: './comite-tecnico.component.html',
  styleUrls: ['./comite-tecnico.component.scss']
})
export class ComiteTecnicoComponent implements OnInit {

  verAyuda = false;

  ordenesDelDia = false;
  sesionComiteTecnico = false;
  gestionActas = false;
  monitoreoCompromisos = false;

  fechaComite: FormControl;
  minDate: Date;

  constructor(
    private router: Router
  ) {
    this.minDate = new Date();
    this.fechaComite = new FormControl('', [Validators.required]);
    this.fechaComite.valueChanges
    .subscribe(value => {
      console.log(value);
    })
  }

  ngOnInit(): void {
  }

}
