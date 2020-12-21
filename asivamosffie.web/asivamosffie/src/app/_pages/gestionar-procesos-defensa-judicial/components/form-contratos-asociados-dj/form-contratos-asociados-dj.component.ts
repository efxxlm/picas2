import { Component, OnInit, ViewChild } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { ContratosModificacionesContractualesService } from 'src/app/core/_services/contratos-modificaciones-contractuales/contratos-modificaciones-contractuales.service';
import { DefensaJudicial, DefensaJudicialService } from 'src/app/core/_services/defensaJudicial/defensa-judicial.service';

@Component({
  selector: 'app-form-contratos-asociados-dj',
  templateUrl: './form-contratos-asociados-dj.component.html',
  styleUrls: ['./form-contratos-asociados-dj.component.scss']
})
export class FormContratosAsociadosDjComponent implements OnInit {
  displayedColumns: string[] = ['nombreContratista', 'institucionEducativa', 'codigoDane', 'sede', 'sedeCodigo', 'contratacionProyectoId'];
  dataSource = new MatTableDataSource();
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  dataTable: any[] = [
  ];
  myControl = new FormControl();
  filteredName: Observable<string[]>;
  formContratista: FormGroup;
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
  contratosArray = [];
  contratos=[];
  listProyectos: any[]=[];
  listProyectosSeleccion: any[]=[];

  constructor ( private fb: FormBuilder, private defensaService:DefensaJudicialService ) {
    this.crearFormulario();
  }

  ngOnInit(): void {
    this.defensaService.GetListContract().subscribe(response=>{
      this.contratosArray=response.map(x=>x.numeroContrato);
      this.contratos=response;
      console.log(this.contratosArray);
    });
    this.formContratista.get( 'numeroContratos' ).valueChanges
      .subscribe( value => {
        this.perfiles.clear();
        for ( let i = 0; i < Number(value); i++ ) {
          this.perfiles.push( 
            this.fb.group(
              {
                contrato: [ null ]                
              }
            ) 
          )
        }
      } );

      this.filteredName = this.myControl.valueChanges.pipe(
        startWith(''),
        map(value => this._filter(value))
      );
  };

	  get perfiles () {
		return this.formContratista.get( 'perfiles' ) as FormArray;
	  };

  get numeroRadicado () {
    let numero;
    Object.values( this.formContratista.controls ).forEach( control => {
      if ( control instanceof FormArray ) {
        Object.values( control.controls ).forEach( control => {
          numero = control.get( 'numeroRadicadoFfieAprobacionCv' ) as FormArray;
        } )
      }
    } )
    return numero;
  };

  private _filter(value: string): string[] {
    console.log("intentnado filtrar"+value);
    const filterValue = value.toLowerCase();    
    if(value!="")
    {      
      let filtroportipo:string[]=[];
      this.contratos.forEach(element => {        
        if(element.numeroContrato==value)
        {
          if(!filtroportipo.includes(element.numeroContrato))
          {
            filtroportipo.push(element.numeroContrato);
          }
        }
      });
      let ret= filtroportipo.filter(x=> x.toLowerCase().indexOf(filterValue) === 0);      
      return ret;
    }
    else
    {
      return [];
    }
    
  }

  seleccionAutocomplete(id:any){
    this.perfiles.value.contrato = id;
    let contrato=this.contratos.filter(x=>x.numeroContrato==id);
    this.defensaService.GetListProyectsByContract(contrato[0].contratoId).subscribe(response=>{
      this.listProyectosSeleccion=response;
      this.dataTable=response;
      this.dataSource = new MatTableDataSource(this.dataTable);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    });
    
    
    
  }
  textoLimpio (texto: string) {
    if ( texto ){
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio.length;
    };
  };

  textoLimpioMessage (texto: string) {
    if ( texto ){
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio;
    };
  };

  maxLength (e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    };
  };

  crearFormulario () {
    this.formContratista = this.fb.group({
      numeroContratos: [ '' ],
      perfiles: this.fb.array([])
    });
  };

  eliminarPerfil ( numeroPerfil: number ) {
    this.perfiles.removeAt( numeroPerfil );
    this.formContratista.patchValue({
      numeroContratos: `${ this.perfiles.length }`
    });
  };

  agregarNumeroRadicado () {
    this.numeroRadicado.push( this.fb.control( '' ) )
  }

  eliminarNumeroRadicado ( numeroRadicado: number ) {
    this.numeroRadicado.removeAt( numeroRadicado );
  };

  guardar () {
    console.log( this.formContratista );
    console.log(this.listProyectos);
    let defContraProyecto:any[]=[];
    this.listProyectos.forEach(element => {
      defContraProyecto.push({
        defensaJudicialContratacionProyectoId:0,
        contratacionProyectoId:element,
        esCompleto:true
      });
    });
    
    let defensaJudicial:DefensaJudicial={
      defensaJudicialId:0,
      legitimacionCodigo:'',
      tipoProcesoCodigo:'',
      numeroProceso:'',
      cantContratos:0,
      estadoProcesoCodigo:'',
      solicitudId:0,
      esLegitimacionActiva:null,
      esCompleto:false,
      defensaJudicialContratacionProyecto:defContraProyecto};
      console.log(defensaJudicial);
      this.defensaService.CreateOrEditDefensaJudicial(defensaJudicial).subscribe(
        response=>{
          console.log(response);
        }
      );

  }

  addProject(idproyecto:any)
  {
    
    console.log(idproyecto);
      if(this.listProyectos.includes(idproyecto))
      {
        var index = this.listProyectos.indexOf(idproyecto);
        if (index !== -1) {
          this.listProyectos.splice(index, 1);
        }      
      }
      else
      {
        this.listProyectos.push({contratacionProyectoId:idproyecto});
      }
        
    console.log(this.listProyectos);
  }
}
