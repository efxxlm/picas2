import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { ContractualNoveltyService } from 'src/app/core/_services/ContractualNovelty/contractual-novelty.service';
import { ContratosModificacionesContractualesService } from 'src/app/core/_services/contratos-modificaciones-contractuales/contratos-modificaciones-contractuales.service';
import { NovedadContractual } from 'src/app/_interfaces/novedadContractual';

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

  constructor(
    private contractualNoveltyService: ContractualNoveltyService,
    private activatedRoute: ActivatedRoute,

    ) 
    
  { 
    
  }
  numeroContratoSeleccionado: any;
  proyectos=[];
  proyecto:any;
  contratos=[];
  contrato:any;
  novedad:NovedadContractual = {};

  ngOnInit() {

    this.activatedRoute.params.subscribe( parametros => {
      this.contractualNoveltyService.getNovedadContractualById( parametros.id )
        .subscribe( novedad => {
          console.log( novedad );
          
          this.novedad = novedad;
          this.numeroContrato.setValue( novedad.contrato.numeroContrato );
          this.numeroContratoSeleccionado=novedad.contrato.numeroContrato;
          this.novedadAplicada.setValue( novedad.esAplicadaAcontrato );
          this.proyecto = novedad['proyectosSeleccionado'];
          this.contrato = novedad.contrato;
          
        });

    });

    //traigo contratos
    this.contractualNoveltyService.getContratosAutocomplete().subscribe(respuesta=>{
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
    this.novedad.esAplicadaAcontrato = this.novedadAplicada.value;
    console.log(this.novedadAplicada.value);
    if(this.novedadAplicada.value == false)
    {
      this.contractualNoveltyService.getProyectosContrato(this.numeroContratoSeleccionado.contratoId).subscribe(
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
