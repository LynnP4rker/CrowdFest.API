
using AutoMapper;
using CrowdFest.API.Abstractions.Repositories;
using CrowdFest.API.Entities;
using CrowdFest.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PostController: ControllerBase
{
    private readonly IPostRepository _repository;
    private readonly IMapper _mapper;

    public PostController(IPostRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet("{plannerId:guid}/{postId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PostDto>> RetrievePostAsync(Guid postId, Guid plannerId, CancellationToken cancellationToken)
    {
        PostEntity? postEntity = await _repository.RetrieveAsync(postId, plannerId, cancellationToken);
        if (postEntity is null) return NotFound();
        PostDto post = _mapper.Map<PostDto>(postEntity);
        return Ok(post);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PostDto>>> ListPostsAsync(CancellationToken cancellationToken)
    {
        IEnumerable<PostEntity> postEntities = await _repository.ListAsync(cancellationToken);
        IEnumerable<PostDto> posts = _mapper.Map<IEnumerable<PostDto>>(postEntities);
        return Ok(posts);
    }

    [HttpGet("{plannerId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PostDto>>> ListPostsForPlannerAsync(Guid plannerId, CancellationToken cancellationToken)
    {
        IEnumerable<PostEntity> postEntities = await _repository.ListPlannerPostsAsync(plannerId, cancellationToken);
        IEnumerable<PostDto> posts = _mapper.Map<IEnumerable<PostDto>>(postEntities);
        return Ok(posts);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreatePostAsync([FromBody] PostDto post, CancellationToken cancellationToken)
    {
        PostEntity postEntity = _mapper.Map<PostEntity>(post);
        await _repository.CreateAsync(postEntity, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        PostDto returnedPost = _mapper.Map<PostDto>(postEntity);
        return CreatedAtAction(nameof(RetrievePostAsync), new { id = postEntity.id}, returnedPost);
    }

    [HttpDelete("{plannerId:guid}/{postId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RemovePostAsync(Guid plannerId, Guid postId, CancellationToken cancellationToken)
    {
        PostEntity? postEntity = await _repository.RetrieveAsync(plannerId, postId, cancellationToken);
        if (postEntity is null) return NotFound();
        await _repository.DeleteAsync(postEntity, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    [HttpPut("{plannerId:guid}/{postId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdatePostAsync(Guid plannerId, Guid postId, [FromBody] PostDto post, CancellationToken cancellationToken)
    {
        if ( plannerId != post.plannerId && postId != post.id) return BadRequest ("ID in URL doesn't match the body"); 
        PostEntity? postEntity = await _repository.RetrieveAsync(postId, plannerId, cancellationToken);
        if (postEntity is null) return NotFound();
        _mapper.Map(post, postEntity);
        await _repository.UpdateAsync(postEntity, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        return NoContent();
    }
}