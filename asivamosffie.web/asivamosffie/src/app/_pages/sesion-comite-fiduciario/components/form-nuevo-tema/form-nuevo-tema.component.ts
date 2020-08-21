import { Component, OnInit } from '@angular/core';
import { Temas } from 'src/app/_interfaces/comiteFiduciario.interfaces';

@Component({
  selector: 'app-form-nuevo-tema',
  templateUrl: './form-nuevo-tema.component.html',
  styleUrls: ['./form-nuevo-tema.component.scss']
})
export class FormNuevoTemaComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

  //Agregar tema a tratar en el comite
  agregarTema () {
    //this.temaContador++;
    //this.sesionComiteFiduciario.temas.push({
    //  numeroTema: this.temaContador,
    //  tiempoIntervencion: null,
    //  responsable: '',
    //  urlSoporte: '',
    //  temaTratar: ''
    //});
  };
    
  //Eliminar tema a tratar en el comite
  eliminarTema ( tema: Temas ) {
    //const index = this.sesionComiteFiduciario.temas.findIndex( data => data.numeroTema === tema.numeroTema );
    //this.sesionComiteFiduciario.temas.splice( index, 1 );
  };

}
