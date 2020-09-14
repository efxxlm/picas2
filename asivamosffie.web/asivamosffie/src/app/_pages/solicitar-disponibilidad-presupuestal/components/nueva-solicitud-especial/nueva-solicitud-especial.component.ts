import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { forkJoin } from 'rxjs';
import { BudgetAvailabilityService } from 'src/app/core/_services/budgetAvailability/budget-availability.service';
import { Dominio, CommonService, Localizacion } from 'src/app/core/_services/common/common.service';
import { Aportante, Proyecto } from 'src/app/core/_services/project/project.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { DisponibilidadPresupuestal, DisponibilidadPresupuestalProyecto, ListAportantes } from 'src/app/_interfaces/budgetAvailability';

@Component({
  selector: 'app-nueva-solicitud-especial',
  templateUrl: './nueva-solicitud-especial.component.html',
  styleUrls: ['./nueva-solicitud-especial.component.scss']
})

export class NuevaSolicitudEspecialComponent implements OnInit {

  tipoSolicitudArray: Dominio[] = [];
  listaDepartamento: Localizacion[] = [];
  listaMunicipio: Localizacion[] = [];
  listaAportante: ListAportantes[] = [];
  proyectoEncontrado: boolean = false
  proyecto: Proyecto;

  addressForm = this.fb.group({
    disponibilidadPresupuestalId:[],
    disponibilidadPresupuestalProyectoId:[],

    tipo: [null, Validators.required],
    objeto: [null, Validators.required],
    numeroRadicado: [null, Validators.compose([
      Validators.minLength(10), Validators.maxLength(15)])],
    cartaAutorizacionET: ['', Validators.required],
    numeroContrato: [null, Validators.compose([
      Validators.minLength(3), Validators.maxLength(10)])],
    departemento: [null, Validators.required],
    municipio: [null, Validators.required],
    llaveMEN: [null, Validators.required],
    tipoAportante: [null, Validators.required],
    nombreAportante: [null, Validators.required],
    valor: [null, Validators.compose([
      Validators.minLength(4), Validators.maxLength(20)])],
    url: [null, Validators.required]
  });

  editorStyle = {
    height: '45px'
  };

  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };

  constructor(
    private fb: FormBuilder,
    private commonService: CommonService,
    private budgetAvailabilityService: BudgetAvailabilityService,
    public dialog: MatDialog,
    private router: Router, 
    private activatedRoute: ActivatedRoute,

  ) { }

  cargarRegistro( id: number )
  {
    this.budgetAvailabilityService.getDetailInfoAdditionalById( id )
      .subscribe( disponibilidad => {

        let tipoSeleccionado: Dominio = this.tipoSolicitudArray.find( t => t.codigo == disponibilidad.tipoSolicitudCodigo ); 
        

        console.log( disponibilidad );

        this.addressForm.get('disponibilidadPresupuestalId').setValue(disponibilidad.disponibilidadPresupuestalId);
        this.addressForm.get('tipo').setValue(tipoSeleccionado);
        this.addressForm.get('objeto').setValue( disponibilidad.objeto );
        this.addressForm.get('numeroRadicado').setValue( disponibilidad.numeroRadicadoSolicitud );
        this.addressForm.get('cartaAutorizacionET').setValue( disponibilidad.cuentaCartaAutorizacion );
        
        
        
        if (disponibilidad.disponibilidadPresupuestalProyecto && disponibilidad.disponibilidadPresupuestalProyecto.length > 0){
          this.addressForm.get('disponibilidadPresupuestalProyectoId').setValue(disponibilidad.disponibilidadPresupuestalProyecto[0].disponibilidadPresupuestalProyectoId);
          this.addressForm.get('llaveMEN').setValue(disponibilidad.disponibilidadPresupuestalProyecto[0].proyecto.llaveMen)
          this.buscarProyecto( true, disponibilidad.aportanteId );

        }


      })
  }

  ngOnInit(): void {

    

    forkJoin([
      this.commonService.listaTipoDDPEspecial(),
      this.commonService.listaDepartamentos(),

    ]).subscribe(respuesta => {
      this.tipoSolicitudArray = respuesta[0];
      this.listaDepartamento = respuesta[1];

      this.activatedRoute.params.subscribe( parametros => {
        if ( parametros.id != "0" )
          this.cargarRegistro( parametros.id );
      })

    })

  }

  openDialog(modalTitle: string, modalText: string) {
    const dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  buscarProyecto( esModoEdit: boolean, aportanteId: string ) {
    let llameMen: string = this.addressForm.get('llaveMEN').value;
    console.log(llameMen);
    if (llameMen) {
      this.budgetAvailabilityService.searchLlaveMEN(llameMen)
        .subscribe(listaProyectos => {
          if (listaProyectos.length == 0) {
            this.openDialog('', 'Esta llave no existe por favor verifique los datos registrados');

          } else {
            this.proyectoEncontrado = true;
            this.proyecto = listaProyectos[0];
            console.log(this.proyecto)
            this.budgetAvailabilityService.getAportantesByProyectoId(this.proyecto.proyectoId)
              .subscribe(listaApo => {

                this.listaAportante = listaApo;
                console.log(this.listaAportante)
                if ( esModoEdit ){
                   let aportanteNombreSeleccionado: ListAportantes = this.listaAportante.find( t => t.cofinanciacionAportanteId.toString() == aportanteId ); 
                   this.addressForm.get('nombreAportante').setValue( aportanteNombreSeleccionado );
                }


              })
          }

        }, err => {
          this.openDialog('', err.error.message)
        })
    }
  }

  // evalua tecla a tecla
  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  }

  changeDepartamento() {
    let departamento = this.addressForm.get('departemento').value;
    if (departamento) {
      this.commonService.listaMunicipiosByIdDepartamento(departamento.localizacionId)
        .subscribe(listaMunicipios => {
          this.listaMunicipio = listaMunicipios;
        })
    }
  }

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  textoLimpio(texto: string) {
    const textolimpio = texto.replace(/<[^>]*>/g, '');
    return textolimpio.length;
  }

  changeAportante() {
    this.addressForm.get('valor').setValue(this.addressForm.get('nombreAportante').value.valorAporte);
  }

  onSubmit() {
    //if (this.addressForm.valid) {

      let tipoDDP: Dominio = this.addressForm.get('tipo').value;


      if (tipoDDP) {

        switch (tipoDDP.codigo) {
          case "1":

            let disponibilidad: DisponibilidadPresupuestal = {

              disponibilidadPresupuestalId: this.addressForm.get('disponibilidadPresupuestalId').value,
              tipoSolicitudCodigo: tipoDDP.codigo,
              objeto: this.addressForm.get('objeto').value,
              numeroRadicadoSolicitud: this.addressForm.get('numeroRadicado').value,
              aportanteId: this.addressForm.get('nombreAportante').value ? this.addressForm.get('nombreAportante').value.cofinanciacionAportanteId : null,
              valorSolicitud: this.addressForm.get('valor').value,
              valorAportante: this.addressForm.get('valor').value,
              cuentaCartaAutorizacion: this.addressForm.get('cartaAutorizacionET').value,
              urlSoporte: this.addressForm.get('url').value,

              disponibilidadPresupuestalProyecto: []

            }

            if (this.proyecto){
              let disponibilidadPresupuestalProyecto: DisponibilidadPresupuestalProyecto = {
                disponibilidadPresupuestalId: this.addressForm.get('disponibilidadPresupuestalId').value,
                proyectoId: this.proyecto.proyectoId,
                
              }

              disponibilidad.disponibilidadPresupuestalProyecto.push( disponibilidadPresupuestalProyecto );
            }
            

            this.budgetAvailabilityService.createOrEditDDPRequest( disponibilidad )
              .subscribe( respuesta => {
                this.openDialog( '', respuesta.message )
                if ( respuesta.code == "200" )
                  this.router.navigate(['/solicitarDisponibilidadPresupuestal/crearSolicitudEspecial'])
              })

            console.log(disponibilidad);


            break;
        }




      }
    //}
  }
}
