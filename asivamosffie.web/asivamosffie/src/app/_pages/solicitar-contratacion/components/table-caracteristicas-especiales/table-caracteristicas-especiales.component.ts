import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Contratacion } from 'src/app/_interfaces/project-contracting';
import { Router } from '@angular/router';

@Component({
  selector: 'app-table-caracteristicas-especiales',
  templateUrl: './table-caracteristicas-especiales.component.html',
  styleUrls: ['./table-caracteristicas-especiales.component.scss']
})
export class TableCaracteristicasEspecialesComponent implements OnInit {

  @Input() contratacion: Contratacion

  displayedColumns: string[] = [
    'tipoInterventor',
    'llaveMEN',
    'region',
    'departamento',
    'institucionEducativa',
    'sede',
    'id'
  ];

  dataSource = new MatTableDataSource();

  @ViewChild(MatSort, { static: true }) sort: MatSort;

  constructor(private routes: Router) { }

  ngOnInit(): void {
    //this.dataSource = new MatTableDataSource();
    this.dataSource.sort = this.sort;
  }

  cargarRegistros() {
    this.dataSource = new MatTableDataSource(this.contratacion.contratacionProyecto);
  }

  definirCaracteristicas(id: number, municipio: any) {
    this.routes.navigate(['/solicitarContratacion/definir-caracteristicas', id], { state: { municipio: municipio } })
  }

  getSemaforo(elemento: any, contratacionProyecto: any) {

    let caracteristicasconalgo = false;
    let registroCompleto: boolean = true;

    if ( contratacionProyecto !== undefined ){
      if (
        contratacionProyecto['tieneMonitoreoWeb'] !== undefined ||
        contratacionProyecto['esReasignacion'] !== undefined ||
        contratacionProyecto['esAvanceobra'] !== undefined ||
        contratacionProyecto['porcentajeAvanceObra'] !== undefined || 
        contratacionProyecto['requiereLicencia'] !== undefined ||
        contratacionProyecto['numeroLicencia'] !== undefined ||
        contratacionProyecto['licenciaVigente'] != undefined || 
        contratacionProyecto['fechaVigencia'] !== undefined
        
      ) 
      {
        caracteristicasconalgo = true;
      }
  
      if (
        contratacionProyecto['tieneMonitoreoWeb'] === undefined ||
        contratacionProyecto['esReasignacion'] === undefined ||
        (contratacionProyecto['esReasignacion'] === true && contratacionProyecto['esAvanceobra'] === undefined) ||
        (contratacionProyecto['esAvanceobra'] === true && contratacionProyecto['porcentajeAvanceObra'] === undefined) ||
        (contratacionProyecto['porcentajeAvanceObra'] !== undefined && contratacionProyecto['requiereLicencia'] === undefined) ||
        (contratacionProyecto['requiereLicencia'] === true && contratacionProyecto['licenciaVigente'] === undefined) ||
        (contratacionProyecto['licenciaVigente'] === true && contratacionProyecto['numeroLicencia'] === undefined) ||
        (contratacionProyecto['licenciaVigente'] === true && contratacionProyecto['fechaVigencia'] === undefined) ||
  
        (contratacionProyecto['esReasignacion'] === false && contratacionProyecto['requiereLicencia'] === undefined) ||
        (contratacionProyecto['esAvanceobra'] === false && contratacionProyecto['requiereLicencia'] === undefined)
      ) 
      {
        registroCompleto = false;
      }
    }
    

    let respuesta: string = '';

    if (caracteristicasconalgo == true) {// && tieneMonitoreoWeb !== undefined ) {
      respuesta = 'en-proceso';
    }
    if (caracteristicasconalgo == false) {// && tieneMonitoreoWeb !== undefined ) {
      respuesta = 'sin-diligenciar';
    }
    if (registroCompleto === true ){
      respuesta = 'completo';
    }

    return respuesta
  };

}
