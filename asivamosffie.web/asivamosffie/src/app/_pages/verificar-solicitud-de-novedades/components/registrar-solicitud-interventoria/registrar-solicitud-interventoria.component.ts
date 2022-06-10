import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { ContractualNoveltyService } from 'src/app/core/_services/ContractualNovelty/contractual-novelty.service';
import { ContratosModificacionesContractualesService } from 'src/app/core/_services/contratos-modificaciones-contractuales/contratos-modificaciones-contractuales.service';
import { NovedadContractual } from 'src/app/_interfaces/novedadContractual';
import { Router } from '@angular/router';

@Component({
  selector: 'app-registrar-solicitud-interventoria',
  templateUrl: './registrar-solicitud-interventoria.component.html',
  styleUrls: ['./registrar-solicitud-interventoria.component.scss']
})
export class RegistrarSolicitudInterventoriaComponent implements OnInit {
  
  listaTipoDocumento: Dominio[] = [];
  numeroContratoSeleccionado: any;
  proyectos=[];
  proyecto:any;
  contratos=[];
  contrato:any;
  novedad:NovedadContractual = {};

  numeroContrato = new FormControl();
  novedadAplicada = new FormControl();
  filteredOptions: Observable<string[]>;
  //options: string[] = [];
  options: any[] = [];

  novedadesArray = [
    { name: 'Contrato', value: true },
    { name: 'Proyecto', value: false }
  ];
  estaEditando = false;
  detalleId: any;

  constructor(
    private contractualNoveltyService: ContractualNoveltyService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private commonSvc: CommonService,
    )

  {

  }


 async ngOnInit() {
    const listaDocumento = await this.commonSvc.listaTipodocumento().toPromise()
    this.listaTipoDocumento = listaDocumento


    this.activatedRoute.params.subscribe( parametros => {
      this.detalleId = parametros.id;
      this.contractualNoveltyService.getNovedadContractualById( parametros.id )
        .subscribe( novedad => {
 
        
       
          if (novedad.novedadContractualId !== 0) {

            console.log( novedad )
            this.novedad = novedad;
            this.numeroContrato.setValue(novedad.contrato.numeroContrato);
            this.numeroContratoSeleccionado = novedad.contrato;
            this.novedadAplicada.setValue(novedad.esAplicadaAcontrato);
            this.proyecto = novedad['proyectosSeleccionado'];
            this.contrato = novedad.contrato;
            this.contrato.datosContratoProyectoModificadosXNovedad = this.novedad.datosContratoProyectoModificadosXNovedad;

            if (this.novedadAplicada.value == false) {
              this.contractualNoveltyService.getProyectosContrato(this.contrato.contratoId).subscribe(
                response => {
                  this.proyectos = response;
                  console.log(this.proyectos);

                }
              );
            } else {
              this.proyecto = null;
            }

            if (this.contrato !== undefined) {
              this.contratos.push(this.contrato)
              this.options.push(this.contrato)
            }

            this.estaEditando = true;
            this.numeroContrato.markAllAsTouched();
            this.novedadAplicada.markAllAsTouched();
          }

        });

    });

    //traigo contratos
    this.contractualNoveltyService.getContratosAutocomplete().subscribe(respuesta=>{
      this.contratos=respuesta.filter( r => r.tipoSolicitudCodigo === '2' ); // interventoria
      //this.options=respuesta.map(function(task,index,array){return task.numeroContrato})
      this.options=respuesta.filter( r => r.tipoSolicitudCodigo === '2' ); // interventoria
    });
    this.filteredOptions = this.numeroContrato.valueChanges.pipe(
      startWith(''),
      map(value => this._filter(value))
    );

  }
  findTipoDocumentoById( codigo: string ) {
    if ( this.listaTipoDocumento.length > 0 ) {
        const tipoDocumento = this.listaTipoDocumento.find( td => td.codigo === codigo )

        if ( tipoDocumento ) 
          return tipoDocumento.nombre
    }
}

  guardar() {
    if (this.detalleId !== 0) {
      this.ngOnInit();
    }
  }

  private _filter(value: string): string[] {
    console.log( typeof value )
    const filterValue = value.toLowerCase();

    return this.options.filter(option => option.numeroContrato.toLowerCase().indexOf(filterValue) === 0);
  }

  public seleccionAutocomplete(numeroContrato)
  {
    if(numeroContrato.esMultiProyecto == true){
      this.novedadesArray = this.novedadesArray.filter(r => r.name == "Proyecto");
    }else{
      if(!this.novedadesArray.find(r =>r.name == "Contrato")){
        this.novedadesArray.push({ name: 'Contrato', value: true });
      }
    }
    this.numeroContratoSeleccionado=numeroContrato;
    this.contrato = null;
    this.proyecto = null;
    this.novedadAplicada.setValue(null);
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
    this.contractualNoveltyService.getDatosContratoProyectoModificadosXNovedad(this.proyecto.proyectoId, this.contrato.contratoId).subscribe(respuesta => {
      this.contrato.datosContratoProyectoModificadosXNovedad = respuesta;
    });
  }

}
