import { Component, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-comite-fiduciario',
  templateUrl: './comite-fiduciario.component.html',
  styleUrls: ['./comite-fiduciario.component.scss']
})
export class ComiteFiduciarioComponent implements OnInit {

  verAyuda = false;

  ordenesDelDia = false;
  sesionComiteFiduciario = false;
  gestionActas = false;
  monitoreoCompromisos = false;

  fechaComite: FormControl;
  minDate: Date;

  constructor(
              private router: Router,

             ) 
  {
    this.minDate = new Date();
    this.fechaComite = new FormControl('', [Validators.required]);
    this.fechaComite.valueChanges
    .subscribe(value => {
      console.log(value);
    })
  }

  ngOnInit(): void {
  }

  onClickCrearOrden(){
    this.router.navigate( ['/comiteFiduciario/crearOrdenDelDia', 1 ], { state: { fecha: this.fechaComite.value } } );
  };

}
