import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-mapa-interactivo',
  templateUrl: './mapa-interactivo.component.html',
  styleUrls: ['./mapa-interactivo.component.scss']
})
export class MapaInteractivoComponent implements OnInit {
  addressForm = this.fb.group({
    quieroVer: [ null ],
  });
  myFilter = new FormControl();
  listasMapas = [
    {
      name: 'Estadísticas por Entidad Territorial o Institución Educativa',
      value: 'Estadísticas por Entidad Territorial o Institución Educativa',
      code: '1'
    },
    {
      name: 'Estadísticas de contratistas',
      value: 'Estadísticas de contratistas',
      code: '2'
    },
    {
      name: 'Estadísticas de supervisores y apoyo a la supervisión',
      value: 'Estadísticas de supervisores y apoyo a la supervisión',
      code: '3'
    }
  ];
  idTabla: any;
  //estaEditando = false;
  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
  }
  selectedOption(id){
    console.log(id);
    this.idTabla = id;
  }
}
