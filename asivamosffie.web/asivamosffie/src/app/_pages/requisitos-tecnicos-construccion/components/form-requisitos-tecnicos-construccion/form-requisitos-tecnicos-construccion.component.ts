import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { DialogCargarProgramacionComponent } from '../dialog-cargar-programacion/dialog-cargar-programacion.component';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-form-requisitos-tecnicos-construccion',
  templateUrl: './form-requisitos-tecnicos-construccion.component.html',
  styleUrls: ['./form-requisitos-tecnicos-construccion.component.scss']
})
export class FormRequisitosTecnicosConstruccionComponent implements OnInit {

  probBoolean: boolean = false;

  constructor ( private dialog: MatDialog ) {}

  ngOnInit(): void {
  };

}
