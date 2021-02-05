import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { ContratosModificacionesContractualesService } from 'src/app/core/_services/contratos-modificaciones-contractuales/contratos-modificaciones-contractuales.service';

@Component({
  selector: 'app-registrar-solicitud',
  templateUrl: './registrar-solicitud.component.html',
  styleUrls: ['./registrar-solicitud.component.scss']
})
export class RegistrarSolicitudComponent implements OnInit {

  numeroContrato = new FormControl();
  novedadAplicada = new FormControl();
  filteredOptions: Observable<string[]>;
  //options: string[] = [];
  options: any[] = [];

  novedadesArray = [
    { name: 'Contrato', value: true },
    { name: 'Proyecto', value: false }
  ];

  constructor(public contratoServices: ContratosModificacionesContractualesService) { 
    
  }
  numeroContratoSeleccionado: any;
  proyectos=[];
  proyecto:null;
  contratos=[];
  contrato:null;

  ngOnInit() {
    //traigo contratos
    this.contratoServices.getContratosAutocomplete().subscribe(respuesta=>{
      this.contratos=respuesta;
      //this.options=respuesta.map(function(task,index,array){return task.numeroContrato})
      this.options=respuesta;
    });
    this.filteredOptions = this.numeroContrato.valueChanges.pipe(
      startWith(''),
      map(value => this._filter(value))
    );
    
  }

  private _filter(value: string): string[] {
    console.log( typeof value )
    const filterValue = value.toLowerCase();

    return this.options.filter(option => option.numeroContrato.toLowerCase().indexOf(filterValue) === 0);
  }

  public seleccionAutocomplete(numeroContrato)
  {
    this.numeroContratoSeleccionado=numeroContrato;
  }

  public changeNovedadAplicada()
  {
    console.log(this.novedadAplicada.value);
    if(this.novedadAplicada.value == false)
    {
      this.contratoServices.getProyectosContrato(this.numeroContratoSeleccionado.contratoId).subscribe(
        response=>
        {
          this.proyectos=response;
          console.log(this.proyectos);

        }
      );
    }else{
      this.proyecto = null;
    }
    this.contrato=this.contratos.filter(x=>x.numeroContrato==this.numeroContratoSeleccionado.numeroContrato)[0];
    console.log(this.contrato, this.proyecto);
  }

  seleccionarProyecto( proy ){
    console.log(proy, this.contrato)
    this.proyecto = proy;
  }

}
